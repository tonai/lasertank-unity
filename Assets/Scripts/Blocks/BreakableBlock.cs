using System;
using UnityEngine;

public class BreakableBlock : Block
{
    public override bool ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        Destroy(this.gameObject);
        shooter.DestroyLaser();
        callback();
        return true;
    }
}
