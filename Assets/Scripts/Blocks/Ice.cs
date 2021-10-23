using System;
using UnityEngine;

public class Ice : Block
{
    public int breachLevel = -1;

    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();
        Movable movable = gameObject.GetComponent<Movable>();
        if (movable)
        {
            movable.Move(block.direction, () => {
                if (breachLevel > 0)
                {
                    breachLevel--;
                }
                if (breachLevel == 0)
                {
                    boardManager.ReplaceBlock(this.gameObject, 18);
                }
                callback();
            });
        }

        return true;
    }
}
