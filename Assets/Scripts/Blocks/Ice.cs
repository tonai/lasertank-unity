using System;
using UnityEngine;

public class Ice : Block
{
    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();
        Movable movable = gameObject.GetComponent<Movable>();
        if (movable)
        {
            movable.Move(block.direction, callback);
        }

        return true;
    }
}
