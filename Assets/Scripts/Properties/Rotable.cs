using System;
using UnityEngine;

public class Rotable : MonoBehaviour
{
    public float speed = 5f;

    private Block block;
    private bool isKeyDown = false;
    private Direction? nextDirection = null;
    private float? nextRotation = null;

    public void Start()
    {
        block = GetComponent<Block>();
    }

    public void Update()
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
    }

    public bool IsRotating()
    {
        return nextRotation != null;
    }

    public void Rotate(float rotation)
    {
        nextRotation = (float)Math.Round(transform.rotation.eulerAngles.y) + rotation;
        Direction direction = block.GetDirection();
        Debug.Log(nextRotation);

        if (rotation == 90f)
        {
            nextDirection = DirectionHelper.GetClockwiseDirection(direction);
        }
        else if(rotation == -90f)
        {
            nextDirection = DirectionHelper.GetCounterClockwiseDirection(direction);
        }
        else
        {
            nextDirection = DirectionHelper.GetOppositeDirection(direction);
        }
    }

    public void SetKeyDown(bool isKeyDown)
    {
        this.isKeyDown = isKeyDown;
    }

    private void EndRotation()
    {
        block.SetDirection((Direction)nextDirection);
        nextDirection = null;
        nextRotation = null;
    }
}
