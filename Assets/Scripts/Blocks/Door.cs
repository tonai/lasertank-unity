using System;
using UnityEngine;

public class Door : Block
{
    public int index = 0;

    public override bool CanMoveThrough(GameObject gameObject)
    {
        Opener opener = gameObject.GetComponent<Opener>();

        if (opener && opener.CanOpenDoor(index))
        {
            return true;
        }

        return false;
    }

    public override bool MoveStart(GameObject gameObject)
    {
        Opener opener = gameObject.GetComponent<Opener>();

        if (opener && opener.CanOpenDoor(index))
        {
            opener.OpenDoor(index);
            Destroy(this.gameObject);
        }

        return false;
    }
}
