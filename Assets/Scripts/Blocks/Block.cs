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

    protected BlockType blockType;
    protected BoardManager boardManager;
    protected Vector2Int position;

    public virtual void Start()
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

    public BlockType GetBlockType()
    {
        return blockType;
    }

    public virtual Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
        continueShooting = canShootThrough;
        return DirectionHelper.GetShootHitPosition(position, DirectionHelper.GetOppositeDirection(direction), yOffset, boardManager.tileSize);
    }

    public virtual void MoveOut() { }

    public virtual bool MoveOver(GameObject gameObject, Action callback)
    {
        return false;
    }

    public void SetBlockType(BlockType blockType)
    {
        this.blockType = blockType;
    }

    public void SetPosition(int x, int z)
    {
        position = new Vector2Int(x, z);
    }

    public virtual bool ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        return false;
    }
}
