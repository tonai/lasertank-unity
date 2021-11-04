using System;
using System.Collections;
using UnityEngine;

public class Rotable : MonoBehaviour
{
    public float speed = 360f;

    private Block block;
    private bool isRotating = false;

    public void Start()
    {
        block = GetComponent<Block>();
    }

    public bool IsRotating()
    {
        return isRotating;
    }

    public void Rotate(float rotation, bool saveHistory = false, Action callback = null)
    {
        if (saveHistory) {
            HistoryManager.current.Push();
        }
        float nextRotation = (float)Math.Round(transform.rotation.eulerAngles.y) + rotation;
        StartCoroutine(AnimateRotation(nextRotation, callback));
    }

    private IEnumerator AnimateRotation(float nextRotation, Action callback = null)
    {
        isRotating = true;
        float startPosition = transform.rotation.eulerAngles.y;
        float endPosition = nextRotation;
        float newPosition = startPosition;
        float startTime = Time.time;
        float distance = Math.Abs(nextRotation - startPosition);
        float duration = distance / speed;

        while (newPosition != endPosition)
        {
            float t = (Time.time - startTime) / duration;
            newPosition = Mathf.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newPosition, transform.rotation.eulerAngles.z);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, endPosition, transform.rotation.eulerAngles.z);
        RotateEnd(DirectionHelper.GetDirection(nextRotation - block.rotationOffset), callback);
    }

    private void RotateEnd(Direction newDirection, Action callback = null)
    {
        block.direction = newDirection;
        RotateEndCallback(callback);
    }

    private void RotateEndCallback(Action callback = null)
    {
        isRotating = false;
        if (callback != null)
        {
            callback();
        }
    }
}
