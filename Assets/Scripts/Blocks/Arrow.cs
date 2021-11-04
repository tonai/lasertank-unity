using System;
using UnityEngine;

public class Arrow : Block
{
    public override bool MoveEnd(GameObject gameObject, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();
        if (block.id != 100)
        {
            callback();
            return true;
        }

        Movable movable = gameObject.GetComponent<Movable>();
        if (movable)
        {
            movable.Move(direction, false, callback);
        }

        return false;
    }
}
