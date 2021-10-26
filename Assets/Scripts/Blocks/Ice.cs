using System;
using UnityEngine;

public class Ice : Block
{
    public int breachLevel = -1;

    public override void MoveOut()
    {
        if (breachLevel > 0)
        {
            breachLevel--;
        }
        if (breachLevel == 0)
        {
            boardManager.ReplaceBlock(this.gameObject, 18);
        }
    }

    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        Movable movable = gameObject.GetComponent<Movable>();

        if (movable)
        {
            movable.Move(movable.GetMoveDirection(), callback);
        }

        return true;
    }
}
