using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode right;
    public KeyCode left;
    public KeyCode shoot;
    public bool useCardinalPointsMovement = false;

    private Block block;
    private Movable movable;
    private Rotable rotable;
    private Shooter shooter;

    void Start()
    {
        block = GetComponent<Block>();
        movable = GetComponent<Movable>();
        rotable = GetComponent<Rotable>();
        shooter = GetComponent<Shooter>();
    }

    void Update()
    {
        Direction? targetDirection = null;
        bool canPerformAction = !movable.IsMoving() && !rotable.IsRotating() && !shooter.IsShooting();

        if (Input.GetKeyDown(shoot))
        {
            if (canPerformAction)
            {
                Direction direction = block.direction;
                shooter.Shoot(block.GetPosition(), direction);
                return;
            }
        }

        if (Input.GetKey(forward))
        {
            targetDirection = Direction.North;
        }
        if (Input.GetKey(backward))
        {
            targetDirection = Direction.South;
        }
        if (Input.GetKey(right))
        {
            targetDirection = Direction.Est;
        }
        if (Input.GetKey(left))
        {
            targetDirection = Direction.West;
        }

        if (targetDirection != null && canPerformAction)
        {
            if (useCardinalPointsMovement)
            {
                HandleCardinalPointsMovement((Direction)targetDirection);
            }
            else
            {
                HandleRelativeMovement((Direction)targetDirection);
            }
        }
    }

    private void HandleRelativeMovement(Direction targetDirection)
    {
        Direction direction = block.direction;

        switch (targetDirection)
        {
            case Direction.North:
                movable.Move(direction);
                break;

            case Direction.South:
                movable.Move(DirectionHelper.GetOppositeDirection(direction));
                break;

            case Direction.Est:
                rotable.Rotate(90f);
                break;

            case Direction.West:
                rotable.Rotate(-90f);
                break;

        }
    }

    private void HandleCardinalPointsMovement(Direction targetDirection)
    {
        Direction direction = block.direction;

        if (direction == targetDirection)
        {
            movable.Move(direction);
        }
        else
        {
            rotable.Rotate(DirectionHelper.GetRotation(direction, targetDirection, transform.rotation.eulerAngles.y));
        }
    }
}
