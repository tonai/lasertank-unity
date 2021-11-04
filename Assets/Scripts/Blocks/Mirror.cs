using UnityEngine;

public class Mirror : Block
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

        return base.GetShootHitPosition(yOffset, ref direction, ref continueShooting);
    }
}
