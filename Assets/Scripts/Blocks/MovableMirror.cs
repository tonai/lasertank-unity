using System;
using UnityEngine;

public class MovableMirror : Block
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

    public override void ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        if (direction == this.direction || direction == DirectionHelper.GetClockwiseDirection(this.direction))
        { 
            Movable movable = GetComponent<Movable>();
            if (movable)
            {
                HistoryManager.current.Push();
                Vector3 endPosition = DirectionHelper.GetShootHitPosition(position, direction, 0, BoardManager.current.tileSize);
                shooter.AddLaserPoint(new Vector2(endPosition.x, endPosition.z), movable.speed, shooter.DestroyLaser);
                movable.Move(direction, false, callback);
                return;
            }
        }

        base.ShootThrough(gameObject, direction, shooter, callback);
    }
}
