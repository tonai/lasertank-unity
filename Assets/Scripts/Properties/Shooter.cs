using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject canon;
    public GameObject laserPrefab;
    public float speed = 10f;

    private BoardManager boardManager;
    private float canonYOffset;
    private Block hitBlock;
    private Direction hitDirection;
    private bool isShooting = false;
    private GameObject laser;
    private List<Vector3> trajectoryList;

    public void Start()
    {
        GameObject boardManagerInstance = GameObject.FindWithTag("BoardManager");
        if (boardManagerInstance != null)
        {
            boardManager = boardManagerInstance.GetComponent<BoardManager>();
        }
    }

    public void AddLaserPoint(Vector2 position, float speed, Action callback)
    {
        LineRenderer line = laser.GetComponent<LineRenderer>();
        int pointsCount = line.positionCount + 1;
        line.positionCount = pointsCount;
        int i = pointsCount - 2;
        Vector3 startPosition = line.GetPosition(i);
        Vector3 endPosition = new Vector3(position.x, startPosition.y, position.y);
        StartCoroutine(AnimateLineSegment(line, i, pointsCount, startPosition, endPosition, speed, callback));
    }

    public void DestroyLaser()
    {
        Destroy(laser);
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public void SetIsShooting(bool isShooting)
    {
        this.isShooting = isShooting;
    }

    public void Shoot(Vector2Int position, Direction direction, Action callback = null)
    {
        isShooting = true;
        trajectoryList = new List<Vector3>();
        canonYOffset = canon.transform.position.y;
        trajectoryList.Add(canon.transform.position);

        Vector2Int nextPosition = DirectionHelper.GetNextPosition(position, direction);
        CheckNextPosition(nextPosition, direction);
        StartCoroutine(AnimateLine(callback != null ? callback : EndShootCallback));
    }

    private IEnumerator AnimateLine(Action callback)
    {
        laser = Instantiate(laserPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        LineRenderer line = laser.GetComponent<LineRenderer>();
        int pointsCount = trajectoryList.Count;
        line.positionCount = pointsCount;
        laser.SetActive(true);

        for (int i = 0; i < pointsCount - 1; i++)
        {
            Vector3 startPosition = trajectoryList[i];
            Vector3 endPosition = trajectoryList[i + 1];
            yield return StartCoroutine(AnimateLineSegment(line, i, pointsCount, startPosition, endPosition, speed));
        }

        EndShoot(callback);
    }

    private IEnumerator AnimateLineSegment(LineRenderer line, int i, int pointsCount, Vector3 startPosition, Vector3 endPosition, float speed, Action callback = null)
    {
        float startTime = Time.time;
        Vector3 pos = startPosition;
        float distance = Vector3.Distance(startPosition, endPosition);
        float segmentDuration = distance / speed;
        line.SetPosition(i, startPosition);

        while (pos != endPosition)
        {
            float t = (Time.time - startTime) / segmentDuration;
            pos = Vector3.Lerp(startPosition, endPosition, t);
            line.SetPosition(i + 1, pos);

            for (int j = i + 1; j < pointsCount; j++)
            {
                line.SetPosition(j, pos);
            }

            yield return null;
        }

        if (callback != null)
        {
            callback();
        }
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

                if (objectBlock != null)
                {
                    hitBlock = objectBlock;
                    hitDirection = direction;
                    Vector3 trajectoryPosition = objectBlock.GetShootHitPosition(canonYOffset, ref direction, ref continueShooting);
                    trajectoryList.Add(trajectoryPosition);
                }
            }
        }

        if (continueShooting)
        {
            Vector2Int nextPosition = DirectionHelper.GetNextPosition(position, direction);
            CheckNextPosition(nextPosition, direction);
        }
    }

    private void EndShoot(Action callback)
    {
        if (!hitBlock || !hitBlock.ShootThrough(gameObject, hitDirection, this, callback))
        {
            DestroyLaser();
            callback();
        }
    }

    private void EndShootCallback()
    {
        isShooting = false;
    }
}
