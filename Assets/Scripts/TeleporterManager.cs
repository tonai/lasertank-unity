using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public static TeleporterManager current;

    private static Dictionary<int, List<string>> instances = new Dictionary<int, List<string>>();

    private void Awake()
    {
        current = this;
    }

    public void AddTeleporter(GameObject teleporter)
    {
        Block block = teleporter.GetComponent<Block>();

        if (block != null) {
            List<string> teleporters;
            if (!instances.TryGetValue(block.id, out teleporters))
            {
                teleporters = new List<string>();
            }

            teleporters.Add(GetKet(block));
            instances[block.id] = teleporters;
        }
    }

    public GameObject GetTeleporter(GameObject teleporter)
    {
        Block block = teleporter.GetComponent<Block>();

        if (block != null)
        {
            List<string> teleporters;

            if (instances.TryGetValue(block.id, out teleporters))
            {
                int index = teleporters.IndexOf(GetKet(block));

                if (index != -1)
                {
                    index += 1;
                    if (index == teleporters.Count)
                    {
                        index = 0;
                    }
                    return GetTeleporter(teleporters[index]);
                }
            }
        }

        return null;
    }

    private string GetKet(Block block)
    {
        Vector2Int position = block.GetPosition();
        return position.x.ToString() + '-' + position.y.ToString();
    }

    private GameObject GetTeleporter(string key)
    {
        string[] parts = key.Split('-');
        Board board = BoardManager.current.GetBoard();
        return board.GetBlock(BlockType.Floor, int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
