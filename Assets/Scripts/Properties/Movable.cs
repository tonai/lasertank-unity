using System;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float speed = 5f;

    private Block block;
    private BoardManager boardManager;
    private bool isKeyDown = false;
    private Vector2? nextPosition = null;

    public void Start()
    {
        block = GetComponent<Block>();

        GameObject boardManagerInstance = GameObject.FindWithTag("BoardManager");
        if (boardManagerInstance != null)
        {
            boardManager = boardManagerInstance.GetComponent<BoardManager>();
        }
    }

    public void Update()
    {
        if (nextPosition != null)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = new Vector3(((Vector2)nextPosition).x, currentPosition.y, ((Vector2)nextPosition).y);
            float distance = Vector3.Distance(targetPosition, currentPosition);

            if (distance > 0)
            {
                Vector3 direction = (targetPosition - currentPosition).normalized;
                Vector3 newPosition = currentPosition + direction * speed * Time.deltaTime * (isKeyDown ? boardManager.tileSize : distance);
                float distanceAfterMoving = Vector3.Distance(targetPosition, newPosition);

                if (distanceAfterMoving > distance || distanceAfterMoving < 0.01f)
                {
                    newPosition = targetPosition;
                    EndMovement((int)Math.Round(newPosition.x / boardManager.tileSize), (int)Math.Round(newPosition.z / boardManager.tileSize));
                }

                transform.position = newPosition;
            }
        }
    }

    public bool IsMoving()
    {
        return nextPosition != null;
    }

    public void Move(Direction direction)
    {
        Vector2Int position = block.GetPosition();
        Vector2 nextPosition = new Vector2(position.x * boardManager.tileSize, position.y * boardManager.tileSize);

        switch (direction)
        {
            case Direction.North:
                nextPosition.x += boardManager.tileSize;
                break;

            case Direction.South:
                nextPosition.x -= boardManager.tileSize;
                break;

            case Direction.Est:
                nextPosition.y -= boardManager.tileSize;
                break;

            case Direction.West:
                nextPosition.y += boardManager.tileSize;
                break;

        }

        int x = (int)Math.Round(nextPosition.x / boardManager.tileSize);
        int y = (int)Math.Round(nextPosition.y / boardManager.tileSize);
        if (CanMoveThroughObject(x, y))
        {
            this.nextPosition = nextPosition;
        }
    }

    public void SetKeyDown(bool isKeyDown)
    {
        this.isKeyDown = isKeyDown;
    }

    private bool CanMoveOverGround(int x, int y)
    {
        Board board = boardManager.GetBoard();
        GameObject groundObject = board.GetGroundBlock(x, y);

        if (groundObject != null)
        {
            Block groundBlock = groundObject.GetComponent<Block>();

            if (groundBlock != null && groundBlock.canMoveOver)
            {
                return true;
            }
        }

        return false;
    }

    private bool CanMoveThroughObject(int x, int y)
    {
        Board board = boardManager.GetBoard();
        GameObject objectObject = board.GetObjectBlock(x, y);

        if (objectObject != null)
        {
            Block objectBlock = objectObject.GetComponent<Block>();

            if (objectBlock != null && objectBlock.canMoveThrough)
            {
                return CanMoveOverGround(x, y);
            }

            return false;
        }

        return CanMoveOverGround(x, y);
    }

    private void EndMovement(int x, int z)
    {
        Board board = boardManager.GetBoard();
        board.SetNewObjectPosition(gameObject, x, z);
        nextPosition = null;
    }
}
