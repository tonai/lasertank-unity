using System;
using UnityEngine;

public class MovableBlock : Block
{
    public override void ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
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

        base.ShootThrough(gameObject, direction, shooter, callback);
    }
}
