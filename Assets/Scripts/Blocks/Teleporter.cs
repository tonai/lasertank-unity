using System;
using UnityEngine;

public class Teleporter : Block
{
    public override bool MoveEnd(GameObject gameObject, Action callback)
    {
        GameObject nextTeleporter = TeleporterManager.current.GetTeleporter(this.gameObject);

        if (nextTeleporter != null) {
            Block teleporterBlock = nextTeleporter.GetComponent<Block>();

            if (teleporterBlock != null)
            {
                Board board = BoardManager.current.GetBoard();
                Vector2Int position = teleporterBlock.GetPosition();
                board.SetNewObjectPosition(gameObject, position.x, position.y);
                gameObject.transform.position += nextTeleporter.transform.position - this.gameObject.transform.position;
                callback();
                return false;
            }
        }

        return base.MoveEnd(gameObject, callback);
    }
}
