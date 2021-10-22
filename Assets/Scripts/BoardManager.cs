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
        InitLevel(board.ground, false, 0);

        // Objects
        InitLevel(board.objects, true, 1);
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

    private void InitLevel(Row[] rows, bool isObject, int level)
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
                            Vector3 position = new Vector3(i * tileSize + block.xOffset, level * tileSize + block.yOffset, j * tileSize + block.zOffset);
                            GameObject instance = InitBlock(prefab, i, j, position, isObject);

                            if (isObject) {
                                board.SetObjectInstance(instance, i, j);
                            }
                            else
                            {
                                board.SetGroundInstance(instance, i, j);
                            }

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

    private GameObject InitBlock(GameObject prefab, int x, int z, Vector3 position,  bool isObject)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        instance.SetActive(true);
        Block block = instance.GetComponent<Block>();

        if (block != null)
        {
            block.SetPosition(x, z);
            block.SetIsObject(isObject);
        }
        return instance;
    }
}
