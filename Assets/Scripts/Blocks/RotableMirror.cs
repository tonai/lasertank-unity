using System;
using UnityEngine;

public class RotableMirror : Block
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

        continueShooting = false;
        return new Vector3(position.x * tileSize, yOffset, position.y * tileSize);
    }

    public override (int, object) Serialize()
    {
        return (id, direction);
    }

    public override void SetState(object state)
    {
        direction = (Direction)state;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, DirectionHelper.GetRotation(direction) + rotationOffset, transform.rotation.eulerAngles.z);
    }

    public override void ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        Rotable rotable = GetComponent<Rotable>();

        if (rotable)
        {
            HistoryManager.current.Push();
            shooter.DestroyLaser();
            rotable.Rotate(90, false, callback);
            return;
        }

        base.ShootThrough(gameObject, direction, shooter, callback);
    }
}
