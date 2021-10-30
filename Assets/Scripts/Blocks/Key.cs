using System;
using UnityEngine;

public class Key : Block
{
    public int index = 0;

    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        Opener opener = gameObject.GetComponent<Opener>();

        if (opener)
        {
            opener.AddKey(index);
            callback();
            Destroy(this.gameObject);
        }

        return true;
    }
}
