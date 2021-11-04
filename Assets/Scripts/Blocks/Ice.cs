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
            BoardManager.current.ReplaceBlock(this.gameObject, 18);
        }
    }

    public override bool MoveEnd(GameObject gameObject, Action callback)
    {
        Movable movable = gameObject.GetComponent<Movable>();

        if (movable)
        {
            movable.Move(movable.GetMoveDirection(), false, callback);
            return false;
        }

        return base.MoveEnd(gameObject, callback);
    }

    public override (int, object) Serialize()
    {
        return (id, breachLevel);
    }

    public override void SetState(object state)
    {
        breachLevel = (int)state;
    }
}
