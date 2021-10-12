using UnityEngine;

public class BreakableBlock : Block
{
    public override bool canShootThrough
    {
        get {
            Destroy(this.gameObject);
            return false;
        }
        set
        {
            _canShootThrough = value;
        }
    }
}
