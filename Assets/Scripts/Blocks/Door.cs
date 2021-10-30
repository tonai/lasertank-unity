using UnityEngine;

public class Door : Block
{
    public int index = 0;

    public override bool CanMoveThrough(GameObject gameObject)
    {
        Opener opener = gameObject.GetComponent<Opener>();

        if (opener && opener.OpenDoor(index))
        {
            Destroy(this.gameObject);
            return true;
        }

        return false;
    }
}
