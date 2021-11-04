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
        Board board = BoardManager.current.GetBoard();
        Block block = teleporter.GetComponent<Block>();

        if (block != null)
        {
            List<string> teleporters;

            if (instances.TryGetValue(block.id, out teleporters))
            {
                int index = teleporters.IndexOf(GetKet(block));
                int nextIndex = index;

                if (nextIndex != -1)
                {
                    do {
                        nextIndex += 1;
                        if (nextIndex == teleporters.Count)
                        {
                            nextIndex = 0;
                        }

                        GameObject nextTeleporter = GetTeleporter(teleporters[nextIndex]);
                        Block nextBlock = nextTeleporter.GetComponent<Block>();
                        if (nextBlock != null)
                        {
                            Vector2Int position = nextBlock.GetPosition();
                            GameObject objectOnTop = board.GetBlock(BlockType.Object, position.x, position.y);
                            
                            if (objectOnTop == null)
                            {
                                return nextTeleporter;
                            }
                        }
                    } while (nextIndex != index);
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
