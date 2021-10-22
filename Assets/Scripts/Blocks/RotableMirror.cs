using System;
using UnityEngine;

public class RotableMirror : Block
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

        continueShooting = false;
        return new Vector3(position.x * boardManager.tileSize, yOffset, position.y * boardManager.tileSize);
    }

    public override bool ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        Rotable rotable = GetComponent<Rotable>();

        if (rotable)
        {
            shooter.DestroyLaser();
            rotable.Rotate(90, callback);
        }

        return true;
    }
}
