using System;
using UnityEngine;

public class MovableMirror : Block
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

        continueShooting = canShootThrough;
        return DirectionHelper.GetShootHitPosition(position, direction, yOffset, boardManager.tileSize);
    }

    public override bool ShootThrough(GameObject gameObject, Direction direction, Action callback)
    {
        Shooter shooter = gameObject.GetComponent<Shooter>();
        Movable movable = GetComponent<Movable>();

        if (movable)
        {
            movable.Move(direction, callback);
        }

        return true;
    }
}
