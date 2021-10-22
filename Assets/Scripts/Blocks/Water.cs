using System;
using System.Collections;
using UnityEngine;

public class Water : Block
{
    public float sinkSpeed = 2f;

    public override bool MoveOver(GameObject gameObject, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();

        if(block.id == 100)
        {
            StartCoroutine(AnimateSink(gameObject.transform, () => Debug.Log("Game over")));
        }
        else if (block.id == 39)
        {
            StartCoroutine(AnimateSink(gameObject.transform, () => ReplaceBlock(gameObject, callback)));
        }
        else
        {
            StartCoroutine(AnimateSink(gameObject.transform, () => {
                Destroy(gameObject);
                callback();
            }));
        }

        return true;
    }

    private IEnumerator AnimateSink(Transform transform, Action callback)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - boardManager.tileSize, startPosition.z);
        Vector3 newPosition = startPosition;
        float startTime = Time.time;
        float distance = Vector3.Distance(endPosition, startPosition);
        float duration = distance / sinkSpeed;

        while (newPosition != endPosition)
        {
            float t = (Time.time - startTime) / duration;
            newPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.position = newPosition;
            yield return null;
        }

        transform.position = endPosition;
        callback();
    }

    private void ReplaceBlock(GameObject gameObject, Action callback)
    {
        Board board = boardManager.GetBoard();
        Destroy(this.gameObject);
        board.SetObjectInstance(null, position.x, position.y);
        board.SetGroundInstance(gameObject, position.x, position.y);
        callback();
    }
}
