using System;
using UnityEngine;

public enum Direction
{
    North, // X positive
    South, // X negative
    Est, // Z positive
    West, // Z negative
}

static class DirectionHelper
{
    static public Direction GetClockwiseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.Est;

            case Direction.South:
                return Direction.West;

            case Direction.Est:
                return Direction.South;

            case Direction.West:
                return Direction.North;

            default:
                return Direction.North; // This should never happen
        }
    }
    static public Direction GetCounterClockwiseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.West;

            case Direction.South:
                return Direction.Est;

            case Direction.Est:
                return Direction.North;

            case Direction.West:
                return Direction.South;

            default:
                return Direction.North; // This should never happen
        }
    }
    static public Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.South;

            case Direction.South:
                return Direction.North;

            case Direction.Est:
                return Direction.West;

            case Direction.West:
                return Direction.Est;

            default:
                return Direction.North; // This should never happen
        }
    }

    static public float GetRotation(Direction direction, Direction targetDirection, float rotation)
    {
        if (direction == targetDirection)
        {
            return 0;
        }
        if (GetClockwiseDirection(direction) == targetDirection)
        {
            return 90f;
        }
        if (GetCounterClockwiseDirection(direction) == targetDirection)
        {
            return -90f;
        }
        if (rotation == 270)
        {
            return -180f;
        }
        return 180f;
    }

    static public Direction GetDirection(float rotation)
    {
        rotation = rotation % 360;
        if (rotation < 0)
        {
            rotation += 360;
        }
        switch (rotation)
        {
            case 0:
                return Direction.North;

            case 180:
                return Direction.South;

            case 90:
                return Direction.Est;

            case 270:
                return Direction.West;

            default:
                return Direction.North; // This should never happen
        }
    }

    static public Vector2Int GetDirectionPosition(Vector2Int position, Direction direction)
    {
        Vector2Int nextPosition = new Vector2Int(position.x, position.y);

        switch (direction)
        {
            case Direction.North:
                nextPosition.x += 1;
                break;

            case Direction.South:
                nextPosition.x -= 1;
                break;

            case Direction.Est:
                nextPosition.y -= 1;
                break;

            case Direction.West:
                nextPosition.y += 1;
                break;
        }

        return nextPosition;

    }

    static public Vector3 GetShootHitPosition(Vector2Int position, Direction direction, float y, float tileSize)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector3((position.x + 0.5f) * tileSize, y, position.y * tileSize);

            case Direction.South:
                return new Vector3((position.x - 0.5f) * tileSize, y, position.y * tileSize);

            case Direction.Est:
                return new Vector3(position.x * tileSize, y, (position.y - 0.5f) * tileSize);

            case Direction.West:
                return new Vector3(position.x * tileSize, y, (position.y + 0.5f) * tileSize);

            default:
                return new Vector3(position.x * tileSize, y, position.y * tileSize); // This should never happen
        }
    }

    static public Vector2Int GetNextPosition(Vector2Int position, Direction direction)
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
}
