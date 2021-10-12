using System;
using System.Collections.Generic;
using UnityEngine;

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
        InitLevel(board.ground, board.SetGroundInstance, 0);

        // Objects
        InitLevel(board.objects, board.SetObjectInstance, 1);
    }

    public Board GetBoard()
    {
        return board;
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

    private void InitLevel(Row[] rows, Action<GameObject, int, int> SetBoardInstance, int level)
    {
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].row != null && rows[i].row.Length > 0)
            {
                for (int j = 0; j < rows[i].row.Length; j++)
                {
                    int id = rows[i].row[j];
                    GameObject prefab;

                    if (prefabDictionary.TryGetValue((int)id, out prefab))
                    {
                        Block block = prefab.GetComponent<Block>();

                        if (block != null)
                        {
                            float y = level * tileSize + block.yOffset;
                            GameObject instance = InitBlock(prefab, i, y, j);
                            SetBoardInstance(instance, i, j);

                            if (id == 100)
                            {
                                InitCamera(instance);
                            }
                        }
                    }
                }
            }
        }
    }

    private GameObject InitBlock(GameObject prefab, int x, float y, int z)
    {
        GameObject instance = Instantiate(prefab, new Vector3(x * tileSize, y, z * tileSize), Quaternion.identity);
        instance.SetActive(true);
        Block block = instance.GetComponent<Block>();

        if (block != null)
        {
            block.SetPosition(x, z);
        }
        return instance;
    }
}
