using System;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Block
{
    static Dictionary<int, List<GameObject>> instances = new Dictionary<int, List<GameObject>>();

    public override void Start()
    {
        base.Start();

        List<GameObject> teleporters;
        if (!instances.TryGetValue(id, out teleporters))
        {
            teleporters = new List<GameObject>();
        }
        teleporters.Add(gameObject);
        instances[id] = teleporters;
    }

    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        List<GameObject> teleporters;
        GameObject teleporter = this.gameObject;

        if (instances.TryGetValue(id, out teleporters))
        {
            int index = teleporters.IndexOf(teleporter);
            if (index != -1)
            {
                index += 1;
                if (index == teleporters.Count)
                {
                    index = 0;
                }

                GameObject nextTeleporter = teleporters[index];
                Block teleporterBlock = nextTeleporter.GetComponent<Block>();
                if (teleporterBlock != null)
                {
                    Board board = boardManager.GetBoard();
                    Vector2Int position = teleporterBlock.GetPosition();
                    board.SetNewObjectPosition(gameObject, position.x, position.y);
                    gameObject.transform.position += nextTeleporter.transform.position - teleporter.transform.position;
                    callback();
                }
            }
        }

        return true;
    }
}
