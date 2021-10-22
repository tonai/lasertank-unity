using System;
using UnityEngine;

public class Arrow : Block
{
    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();
        if (block.id != 100)
        {
            return false;
        }

        Movable movable = gameObject.GetComponent<Movable>();
        if (movable)
        {
            movable.Move(direction, callback);
        }

        return true;
    }
}
