using System;
using UnityEngine;

public class MovableBlock : Block
{
    public override Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
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
