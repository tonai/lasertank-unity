using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float speed = 4f;

    private Block block;
    private bool isMoving = false;
    private Direction moveDirection;
    private Vector2Int startPosition;

    public void Start()
    {
        block = GetComponent<Block>();
    }

    public Direction GetMoveDirection()
    {
        return moveDirection;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Move(Direction direction, bool saveHistory = false, Action callback = null)
    {
        startPosition = block.GetPosition();
        moveDirection = direction;
        Vector2Int nextPosition = DirectionHelper.GetDirectionPosition(startPosition, direction);

        if (CanMoveThroughObject(nextPosition.x, nextPosition.y))
        {
            if (saveHistory)
            {
                HistoryManager.current.Push();
            }

            MoveStart(nextPosition.x, nextPosition.y);

            float tileSize = BoardManager.current.tileSize;
            Vector2 worldPosition = new Vector2(nextPosition.x * tileSize + block.xOffset, nextPosition.y * tileSize + block.zOffset);
            Board board = BoardManager.current.GetBoard();
            board.SetNewObjectPosition(gameObject, nextPosition.x, nextPosition.y);
            StartCoroutine(AnimateMove(worldPosition, callback));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    private IEnumerator AnimateMove(Vector2 nextPosition, Action callback = null)
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

        float tileSize = BoardManager.current.tileSize;
        transform.position = endPosition;
        _ = MoveEnd((int)Math.Round((newPosition.x - block.xOffset) / tileSize), (int)Math.Round((newPosition.z - block.zOffset) / tileSize), callback);
    }

    private bool CanMoveOverFloor(int x, int y)
    {
        Board board = BoardManager.current.GetBoard();
        GameObject floorObject = board.GetFloorBlock(x, y);

        if (floorObject != null)
        {
            Block floorBlock = floorObject.GetComponent<Block>();

            if (floorBlock != null && floorBlock.CanMoveOver(gameObject))
            {
                return CanMoveOverGround(x, y);
            }

            return false;
        }

        return CanMoveOverGround(x, y);
    }

    private bool CanMoveOverGround(int x, int y)
    {
        Board board = BoardManager.current.GetBoard();
        GameObject groundObject = board.GetGroundBlock(x, y);

        if (groundObject != null)
        {
            Block groundBlock = groundObject.GetComponent<Block>();

            if (groundBlock != null && groundBlock.CanMoveOver(gameObject))
            {
                return true;
            }
        }

        return false;
    }

    private bool CanMoveThroughObject(int x, int y)
    {
        Board board = BoardManager.current.GetBoard();
        GameObject objectObject = board.GetObjectBlock(x, y);

        if (objectObject != null)
        {
            Block objectBlock = objectObject.GetComponent<Block>();

            if (objectBlock != null && objectBlock.CanMoveThrough(gameObject))
            {
                return CanMoveOverFloor(x, y);
            }

            return false;
        }

        return CanMoveOverFloor(x, y);
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
        Board board = BoardManager.current.GetBoard();
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

    private async Task MoveEnd(int x, int z, Action callback = null)
    {
        Board board = BoardManager.current.GetBoard();

        GameObject startGround = board.GetGroundBlock(startPosition.x, startPosition.y);
        Block startGroundBlock = startGround.GetComponent<Block>();
        startGroundBlock.MoveOut();

        List<Task> tasks = new List<Task>();
        if (block.id == 100)
        {
            tasks = CheckEnemies();
        }

        bool checkNext = true;
        TaskCompletionSource<bool> floorPromise = new TaskCompletionSource<bool>();
        Action floorResolve = () => floorPromise.SetResult(true);
        GameObject endFloor = board.GetFloorBlock(x, z);
        if (endFloor != null)
        {
            Block endFloorBlock = endFloor.GetComponent<Block>();
            if (endFloorBlock != null)
            {
                checkNext = endFloorBlock.MoveEnd(gameObject, floorResolve);
                tasks.Add(floorPromise.Task);
            }
        }

        if (checkNext)
        {
            TaskCompletionSource<bool> groundPromise = new TaskCompletionSource<bool>();
            Action groundResolve = () => groundPromise.SetResult(true);
            GameObject endGround = board.GetGroundBlock(x, z);
            if (endGround != null)
            {
                Block endGroundBlock = endGround.GetComponent<Block>();
                if (endGroundBlock != null)
                {
                    endGroundBlock.MoveEnd(gameObject, groundResolve);
                    tasks.Add(groundPromise.Task);
                }
            }
        }

        await Task.WhenAll(tasks);
        MoveEndCallback(callback);
    }

    private void MoveEndCallback(Action callback = null)
    {
        isMoving = false;
        if (callback != null)
        {
            callback();
        }
    }

    private void MoveStart(int x, int z)
    {
        bool checkNext = true;
        Board board = BoardManager.current.GetBoard();
        GameObject startObject = board.GetObjectBlock(x, z);
        if (startObject != null)
        {
            Block endObjectBlock = startObject.GetComponent<Block>();
            if (endObjectBlock != null)
            {
                checkNext = endObjectBlock.MoveStart(gameObject);
            }
        }

        if (checkNext)
        {
            GameObject endFloor = board.GetFloorBlock(x, z);
            if (endFloor != null)
            {
                Block endFloorBlock = endFloor.GetComponent<Block>();
                if (endFloorBlock != null)
                {
                    checkNext = endFloorBlock.MoveStart(gameObject);
                }
            }
        }

        if (checkNext)
        {
            GameObject endGround = board.GetGroundBlock(x, z);
            if (endGround != null)
            {
                Block endGroundBlock = endGround.GetComponent<Block>();
                if (endGroundBlock != null)
                {
                    endGroundBlock.MoveStart(gameObject);
                }
            }
        }
    }
}
