using UnityEngine;

public class DoubleMirror : Block
{
    public override Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
        float tileSize = BoardManager.current.tileSize;

        if (direction == DirectionHelper.GetOppositeDirection(this.direction))
        {
            continueShooting = true;
            direction = DirectionHelper.GetClockwiseDirection(this.direction);
            return new Vector3(position.x * tileSize, yOffset, position.y * tileSize);
        }

        if (direction == DirectionHelper.GetCounterClockwiseDirection(this.direction))
        {
            continueShooting = true;
            direction = this.direction;
            return new Vector3(position.x * tileSize, yOffset, position.y * tileSize);
        }

        if (direction == this.direction)
        {
            continueShooting = true;
            direction = DirectionHelper.GetCounterClockwiseDirection(this.direction);
            return new Vector3(position.x * BoardManager.current.tileSize, yOffset, position.y * tileSize);
        }

        continueShooting = true;
        direction = DirectionHelper.GetOppositeDirection(this.direction);
        return new Vector3(position.x * tileSize, yOffset, position.y * tileSize);
    }
}
