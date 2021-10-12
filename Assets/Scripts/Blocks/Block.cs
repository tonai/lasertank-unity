using UnityEngine;

public class Block : MonoBehaviour
{
    public int id = 0;
    public bool canMoveOver = false;
    public bool canMoveThrough = false;
    public virtual bool canShootThrough {
        get
        {
            return _canShootThrough;
        }
        set {
            _canShootThrough = value;
        }
    }
    public float yOffset = 0f;
    public float rotationOffset = 0f;

    [SerializeField] protected bool _canShootThrough = false;
    private Direction direction = Direction.North;
    private Vector2Int position;

    public void OnDestroy()
    {

    }

    public void Start()
    {
        if (rotationOffset != 0f)
        {
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, rotationOffset, transform.rotation.eulerAngles.z);
            transform.rotation = rotation;
        }
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    public void SetPosition(int x, int z, bool isGround = false)
    {
        position = new Vector2Int(x, z);
    }
}
