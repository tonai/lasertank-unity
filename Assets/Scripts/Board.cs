using System;
using UnityEngine;

[Serializable]
public class Row
{
    public int[] row;
    private GameObject[] blocks;

    public GameObject GetBlock(int index)
    {
        if (blocks != null) {
            return blocks[index];
        }
        return null;
    }
    
    public void SetInstance(GameObject block, int index)
    {
        if (blocks == null)
        {
            blocks = new GameObject[row.Length];
        }
        blocks[index] = block;
    }
}

[Serializable]
public class Board
{
    public Row[] ground;
    public Row[] objects;

    public GameObject GetGroundBlock(int x, int z)
    {
        return GetBlock(x, z, ground);
    }


    public GameObject GetObjectBlock(int x, int z)
    {
        return GetBlock(x, z, objects);
    }

    public int GetXSize()
    {
        return ground.Length;
    }

    public int GetZSize()
    {
        return ground[0].row.Length;
    }

    public bool IsPositionInRange(Vector2Int position)
    {
        return !(position.x < 0 || position.x >= GetXSize() || position.y < 0 || position.y >= GetZSize());
    }

    public void RemoveGroundInstance(GameObject block)
    {
        RemoveInstance(block, ground);
    }

    public void RemoveObjectInstance(GameObject block)
    {
        RemoveInstance(block, objects);
    }

    public void SetGroundInstance(GameObject block, int x, int z)
    {
        SetInstance(block, x, z, ground);
    }

    public void SetObjectInstance(GameObject block, int x, int z)
    {
        SetInstance(block, x, z, objects);
    }

    public void SetNewObjectPosition(GameObject gameObject, int x, int z)
    {
        Block block = gameObject.GetComponent<Block>();
        if (block)
        {
            Vector2Int position = block.GetPosition();
            if (position != null)
            {
                SetInstance(null, position.x, position.y, objects);
            }
            block.SetPosition(x, z);
            SetInstance(gameObject, x, z, objects);
        }
    }

    private GameObject GetBlock(int x, int z, Row[] rows)
    {
        if (x < 0 || x >= GetXSize() || z < 0 || z >= GetZSize())
        {
            return null;
        }

        Row row = rows[x];
        if (row == null)
        {
            return null;
        }

        return row.GetBlock(z);
    }

    private void RemoveInstance(GameObject gameObject, Row[] rows)
    {
        Block block = gameObject.GetComponent<Block>();
        if (block)
        {
            Vector2Int position = block.GetPosition();
            if (position != null)
            {
                SetInstance(null, position.x, position.y, rows);
            }
        }
    }

    private void SetInstance(GameObject block, int x, int z, Row[] rows)
    {
        rows[x].SetInstance(block, z);
    }
}
