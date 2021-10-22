using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int id = 0;
    public bool canMoveOver = false;
    public bool canMoveThrough = false;
    public bool canShootThrough = false;
    public Direction direction = Direction.North;
    public float rotationOffset = 0f;
    public float xOffset = 0f;
    public float yOffset = 0f;
    public float zOffset = 0f;

    protected BoardManager boardManager;
    protected bool isObject;
    protected Vector2Int position;

    /*public void OnDestroy()
    {
        Board board = boardManager.GetBoard();

        if (isObject)
        {
            board.SetObjectInstance(null, position.x, position.y);
        }
        else
        {
            board.SetGroundInstance(null, position.x, position.y);

        }
    }*/

    public void Start()
    {
        if (rotationOffset != 0f)
        {
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, rotationOffset, transform.rotation.eulerAngles.z);
            transform.rotation = rotation;
        }

        GameObject boardManagerInstance = GameObject.FindWithTag("BoardManager");
        if (boardManagerInstance != null)
        {
            boardManager = boardManagerInstance.GetComponent<BoardManager>();
        }
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public bool GetIsObject()
    {
        return isObject;
    }

    public virtual Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
        continueShooting = canShootThrough;
        return DirectionHelper.GetShootHitPosition(position, DirectionHelper.GetOppositeDirection(direction), yOffset, boardManager.tileSize);
    }

    public virtual bool MoveOver(GameObject gameObject, Action callback)
    {
        return false;
    }

    public void SetIsObject(bool isObject)
    {
        this.isObject = isObject;
    }

    public void SetPosition(int x, int z)
    {
        position = new Vector2Int(x, z);
    }

    public virtual bool ShootThrough(GameObject gameObject, Direction direction, Action callback)
    {
        return false;
    }
}
