using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float speed = 4f;

    private Block block;
    private BoardManager boardManager;
    private bool isMoving = false;
    private Direction moveDirection;
    private Vector2Int startPosition;

    public void Start()
    {
        block = GetComponent<Block>();

        GameObject boardManagerInstance = GameObject.FindWithTag("BoardManager");
        if (boardManagerInstance != null)
        {
            boardManager = boardManagerInstance.GetComponent<BoardManager>();
        }
    }

    /*public void Update()
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
    }*/

    public Direction GetMoveDirection()
    {
        return moveDirection;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Move(Direction direction, Action callback = null)
    {
        startPosition = block.GetPosition();
        moveDirection = direction;
        Vector2Int nextPosition = DirectionHelper.GetDirectionPosition(startPosition, direction);

        if (CanMoveThroughObject(nextPosition.x, nextPosition.y))
        {
            Board board = boardManager.GetBoard();
            Vector2 worldPosition = new Vector2(nextPosition.x * boardManager.tileSize + block.xOffset, nextPosition.y * boardManager.tileSize + block.zOffset);
            board.SetNewObjectPosition(gameObject, nextPosition.x, nextPosition.y);
            StartCoroutine(AnimateMove(worldPosition, () => EndMovementCallback(callback)));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    private IEnumerator AnimateMove(Vector2 nextPosition, Action callback)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(nextPosition.x, startPosition.y, nextPosition.y);
        Vector3 newPosition = startPosition;
        float startTime = Time.time;
        float distance = Vector3.Distance(endPosition, startPosition);
        float duration = distance / speed;

        while (newPosition != endPosition)
        {
            float t = (Time.time - startTime) / duration;
            newPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.position = newPosition;
            yield return null;
        }

        transform.position = endPosition;
        _ = EndMovement((int)Math.Round((newPosition.x - block.xOffset) / boardManager.tileSize), (int)Math.Round((newPosition.z - block.zOffset) / boardManager.tileSize), callback);
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

    private List<Task> CheckEnemies()
    {
        Task task;
        List<Task> promises = new List<Task>();
        Vector2Int position = block.GetPosition();

        task = CheckEnemiesInDirection(Direction.North, position);
        if (task != null) promises.Add(task);
        task = CheckEnemiesInDirection(Direction.Est, position);
        if (task != null) promises.Add(task);
        task = CheckEnemiesInDirection(Direction.South, position);
        if (task != null) promises.Add(task);
        task = CheckEnemiesInDirection(Direction.West, position);
        if (task != null) promises.Add(task);

        return promises;
    }

    private Task CheckEnemiesInDirection(Direction direction, Vector2Int position)
    {
        Board board = boardManager.GetBoard();
        Vector2Int nextPosition = DirectionHelper.GetNextPosition(position, direction);
        if (!board.IsPositionInRange(nextPosition))
        {
            // Out of range
            return null;
        }

        GameObject objectBlock = board.GetObjectBlock(nextPosition.x, nextPosition.y);
        if (objectBlock == null)
        {
            // No object: continue with next position
            return CheckEnemiesInDirection(direction, nextPosition);
        }

        Block block = objectBlock.GetComponent<Block>();
        Enemy enemy = objectBlock.GetComponent<Enemy>();

        if (enemy != null)
        {
            // Enemy: check if enemy can shoot
            return enemy.CheckShoot(direction);
        }
        else if (block != null && block.canShootThrough)
        {
            // Block: but we can shoot through, so continue with next position
            return CheckEnemiesInDirection(direction, nextPosition);
        }

        // Block: and we can't shoot trhough
        return null;

    }

    private async Task EndMovement(int x, int z, Action callback)
    {
        Board board = boardManager.GetBoard();

        GameObject startGroundBlock = board.GetGroundBlock(startPosition.x, startPosition.y);
        Block startBlock = startGroundBlock.GetComponent<Block>();
        startBlock.MoveOut();

        List<Task> tasks = new List<Task>();
        if (this.block.id == 100)
        {
            tasks = CheckEnemies();
        }

        GameObject endGroundBlock = board.GetGroundBlock(x, z);
        Block endBlock = endGroundBlock.GetComponent<Block>();
        TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
        if (endBlock == null || !endBlock.MoveOver(gameObject, () => promise.SetResult(true)))
        {
            promise.SetResult(true);
        }

        tasks.Add(promise.Task);
        await Task.WhenAll(tasks);
        callback();

    }

    private void EndMovementCallback(Action callback = null)
    {
        isMoving = false;
        if (callback != null)
        {
            callback();
        }
    }
}
