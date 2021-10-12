using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject canon;
    public GameObject laserPrefab;
    public float speed;

    private BoardManager boardManager;
    private bool shooting = false;
    private List<Vector3> trajectoryList;
    private float canonYOffset;

    public void Start()
    {
        GameObject boardManagerInstance = GameObject.FindWithTag("BoardManager");
        if (boardManagerInstance != null)
        {
            boardManager = boardManagerInstance.GetComponent<BoardManager>();
        }
    }


    public bool IsShooting()
    {
        return shooting;
    }

    public void Shoot(Vector2Int position, Direction direction)
    {
        shooting = true;
        trajectoryList = new List<Vector3>();
        canonYOffset = canon.transform.position.y;
        trajectoryList.Add(canon.transform.position);

        Vector2Int nextPosition = GetNextPosition(position, direction);
        CheckNextPosition(nextPosition, direction);
        StartCoroutine(AnimateLine());
    }

    private IEnumerator AnimateLine()
    {
        GameObject laser = Instantiate(laserPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        LineRenderer line = laser.GetComponent<LineRenderer>();
        int pointsCount = trajectoryList.Count;
        laser.SetActive(true);

        for (int i = 0; i < pointsCount - 1; i++)
        {
            float startTime = Time.time;
            Vector3 startPosition = trajectoryList[i];
            Vector3 endPosition = trajectoryList[i + 1];
            Vector3 pos = startPosition;
            float segmentDuration = Vector3.Distance(startPosition, endPosition) / speed;
            line.SetPosition(i, startPosition);

            while (pos != endPosition)
            {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);
                line.SetPosition(i + 1, pos);

                /*for (int j = i + 1; j < pointsCount; j++)
                {
                    line.SetPosition(j, pos);
                }*/

                yield return null;
            }
        }

        EndShoot(laser);
    }

    private void CheckNextPosition(Vector2Int position, Direction direction)
    {
        bool continueShooting = false;
        Board board = boardManager.GetBoard();

        if (board.IsPositionInRange(position))
        {
            continueShooting = true;
            GameObject objectObject = board.GetObjectBlock(position.x, position.y);

            if (objectObject != null)
            {
                Block objectBlock = objectObject.GetComponent<Block>();

                if (objectBlock != null && !objectBlock.canShootThrough)
                {
                    continueShooting = false;
                }
            }
        }

        if (continueShooting)
        {
            Vector2Int nextPosition = GetNextPosition(position, direction);
            CheckNextPosition(nextPosition, direction);
        }
        else
        {
            Vector3 trajectoryPosition = GetTrajectoryPosition(position, DirectionHelper.GetOppositeDirection(direction), canonYOffset);
            trajectoryList.Add(trajectoryPosition);
        }
    }

    private void EndShoot(GameObject laser)
    {
        Destroy(laser);
        shooting = false;
    }

    private Vector2Int GetNextPosition(Vector2Int position, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return position + new Vector2Int(1, 0);

            case Direction.South:
                return position + new Vector2Int(-1, 0);

            case Direction.Est:
                return position + new Vector2Int(0, -1);

            case Direction.West:
                return position + new Vector2Int(0, 1);

            default:
                return position; // This should never happen
        }
    }

    private Vector3 GetTrajectoryPosition(Vector2Int position, Direction direction, float y)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector3((position.x + 0.5f) * boardManager.tileSize, y, position.y * boardManager.tileSize);

            case Direction.South:
                return new Vector3((position.x - 0.5f) * boardManager.tileSize, y, position.y * boardManager.tileSize);

            case Direction.Est:
                return new Vector3(position.x * boardManager.tileSize, y, (position.y - 0.5f) * boardManager.tileSize);

            case Direction.West:
                return new Vector3(position.x * boardManager.tileSize, y, (position.y + 0.5f) * boardManager.tileSize);

            default:
                return new Vector3(position.x * boardManager.tileSize, y, position.y * boardManager.tileSize); // This should never happen
        }
    }
}
