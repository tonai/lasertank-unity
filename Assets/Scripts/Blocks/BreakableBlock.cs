using System;
using UnityEngine;

public class BreakableBlock : Block
{
    public override void ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        HistoryManager.current.Push();
        Destroy(this.gameObject);
        shooter.DestroyLaser();
        callback();
    }
}
