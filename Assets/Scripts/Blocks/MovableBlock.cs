using System;
using UnityEngine;

public class MovableBlock : Block
{
    public override bool ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        Movable movable = GetComponent<Movable>();

        if (movable)
        {
            Vector3 endPosition = DirectionHelper.GetShootHitPosition(position, direction, 0, boardManager.tileSize);
            shooter.AddLaserPoint(new Vector2(endPosition.x, endPosition.z), movable.speed, shooter.DestroyLaser);
            movable.Move(direction, callback);
        }

        return true;
    }
}
