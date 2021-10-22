using System;
using UnityEngine;

public class BreakableBlock : Block
{
    public override bool ShootThrough(GameObject gameObject, Direction direction, Action callback)
    {
        Destroy(this.gameObject);
        callback();
        return true;
    }
}
