using System;
using UnityEngine;

public class Finish : Block
{
    public override bool MoveEnd(GameObject gameObject, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();

        if (block.id == 100)
        {
            Debug.Log("Victory");
        }
        else
        {
            callback();
        }

        return true;
    }
}
