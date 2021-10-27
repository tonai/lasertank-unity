using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoardManager : MonoBehaviour
{
    public TextAsset jsonFile;
    public GameObject[] prefabs;
    public GameObject mainCamera;
    public float tileSize = 1f;

    private Board board;
    private Dictionary<int, GameObject> prefabDictionary = new Dictionary<int, GameObject>();

    public void Start()
    {
        board = JsonUtility.FromJson<Board>(jsonFile.text);

        // Dictionary
        InitDictionary();

        // Ground
        InitLevel(board.ground, BlockType.Ground);

        // Floor
        InitLevel(board.floor, BlockType.Floor);

        // Objects
        InitLevel(board.objects, BlockType.Object);
    }

    public Board GetBoard()
    {
        return board;
    }

    public void RemoveBlock(int x, int z, bool isObject)
    {
        if (isObject)
        {
            board.SetObjectInstance(null, x, z);
        }
        else
        {
            board.SetGroundInstance(null, x, z);
        }
    }

    public void ReplaceBlock(GameObject previousObject, int id)
    {
        Block block = previousObject.GetComponent<Block>();
        if (block != null)
        {
            Vector2Int position = block.GetPosition();
            BlockType blockType = block.GetBlockType();
            CreateBlock(position.x, position.y, id, blockType);
            Destroy(previousObject);
        }
    }

    public void ReplaceBlock(GameObject previousObject, GameObject nextObject)
    {
        Block block = previousObject.GetComponent<Block>();
        if (block != null)
        {
            Vector2Int position = block.GetPosition();
            BlockType blockType = block.GetBlockType();
            board.SetInstance(blockType, nextObject, position.x, position.y);
            Destroy(previousObject);
        }
    }

    private void CreateBlock(int i, int j, int id, BlockType blockType)
    {
        GameObject prefab;
        if (prefabDictionary.TryGetValue(id, out prefab))
        {
            Block block = prefab.GetComponent<Block>();

            if (block != null)
            {
                float yOffset = GetYOffset(blockType);
                Vector3 position = new Vector3(i * tileSize + block.xOffset, yOffset * tileSize + block.yOffset, j * tileSize + block.zOffset);
                GameObject instance = InitBlock(prefab, i, j, position, blockType);
                board.SetInstance(blockType, instance, i, j);

                if (id == 100)
                {
                    InitCamera(instance);
                }
            }
        }
    }

    private float GetYOffset(BlockType blockType)
    {
        switch(blockType)
        {
            case BlockType.Ground:
                return 0;

            case BlockType.Floor:
                return 0.501f;

            case BlockType.Object:
                return 1;

            default:
                return 0;
        }
    }

    private void InitDictionary()
    {
        for(int i = 0; i < prefabs.Length; i++)
        {
            GameObject prefab = prefabs[i];
            Block block = prefab.GetComponent<Block>();
            if (block != null) {
                prefabDictionary.Add(block.id, prefab);
            }
        }
    }

    private void InitCamera(GameObject player)
    {
        // Camera
        FollowCamera followCamera = mainCamera.GetComponent<FollowCamera>();
        if (followCamera != null)
        {
            followCamera.SetPlayer(player);
        }
    }

    private void InitLevel(Row[] rows, BlockType blockType)
    {
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].row != null && rows[i].row.Length > 0)
            {
                for (int j = 0; j < rows[i].row.Length; j++)
                {
                    int id = rows[i].row[j];
                    CreateBlock(i, j, id, blockType);
                }
            }
        }
    }

    private GameObject InitBlock(GameObject prefab, int x, int z, Vector3 position, BlockType blockType)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        instance.SetActive(true);
        Block block = instance.GetComponent<Block>();

        if (block != null)
        {
            block.SetPosition(x, z);
            block.SetBlockType(blockType);
        }
        return instance;
    }
}
