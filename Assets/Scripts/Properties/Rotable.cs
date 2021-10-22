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

    /*public void Update()
    {
        if (nextRotation != null)
        {
            float currentRotation = transform.rotation.eulerAngles.y;
            float targetRotation = (float)nextRotation;

            float distance = Math.Abs(targetRotation - currentRotation);

            if (distance > 0)
            {
                int direction = Math.Sign(targetRotation - currentRotation);
                float newRotation = currentRotation + direction * speed * Time.deltaTime * (isKeyDown ? 90f : distance);
                float distanceAfterMoving = Math.Abs(targetRotation - newRotation);

                if (distanceAfterMoving > distance || distanceAfterMoving < 0.01f || (targetRotation == 0 && newRotation < 0 && direction < 0) || (targetRotation == 360 && newRotation > 360 && direction > 0))
                {
                    newRotation = targetRotation == 360 ? 0 : targetRotation;
                    EndRotation();
                }

                if (currentRotation == 0 && targetRotation < 0)
                {
                    nextRotation += 360;
                }

                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, newRotation, transform.rotation.eulerAngles.z);
                transform.rotation = rotation;
            }
        }
    }*/

    public bool IsRotating()
    {
        return isRotating;
    }

    public void Rotate(float rotation, Action callback = null)
    {
        float nextRotation = (float)Math.Round(transform.rotation.eulerAngles.y) + rotation;
        StartCoroutine(AnimateRotation(nextRotation, callback != null ? callback : EndRotationCallback));
    }

    private IEnumerator AnimateRotation(float nextRotation, Action callback)
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
        EndRotation(DirectionHelper.GetDirection(nextRotation - block.rotationOffset), callback);
    }

    private void EndRotation(Direction newDirection, Action callback)
    {
        block.direction = newDirection;
        callback();
    }

    private void EndRotationCallback()
    {
        isRotating = false;
    }
}
