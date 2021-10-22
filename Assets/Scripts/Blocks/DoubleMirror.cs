using UnityEngine;

public class DoubleMirror : Block
{
    public override Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
        if (direction == DirectionHelper.GetOppositeDirection(this.direction))
        {
            continueShooting = true;
            direction = DirectionHelper.GetClockwiseDirection(this.direction);
            return new Vector3(position.x * boardManager.tileSize, yOffset, position.y * boardManager.tileSize);
        }

        if (direction == DirectionHelper.GetCounterClockwiseDirection(this.direction))
        {
            continueShooting = true;
            direction = this.direction;
            return new Vector3(position.x * boardManager.tileSize, yOffset, position.y * boardManager.tileSize);
        }

        if (direction == this.direction)
        {
            continueShooting = true;
            direction = DirectionHelper.GetCounterClockwiseDirection(this.direction);
            return new Vector3(position.x * boardManager.tileSize, yOffset, position.y * boardManager.tileSize);
        }

        continueShooting = true;
        direction = DirectionHelper.GetOppositeDirection(this.direction);
        return new Vector3(position.x * boardManager.tileSize, yOffset, position.y * boardManager.tileSize);
    }
}
