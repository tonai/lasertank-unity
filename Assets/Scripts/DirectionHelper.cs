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
}
