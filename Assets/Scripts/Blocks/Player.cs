using System;
using UnityEngine;

public class Player : Block
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

    public override void Start()
    {
        base.Start();
        block = GetComponent<Block>();
        movable = GetComponent<Movable>();
        rotable = GetComponent<Rotable>();
        shooter = GetComponent<Shooter>();
    }

    public void Update()
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

    public override Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
        continueShooting = false;
        return new Vector3(position.x * boardManager.tileSize, yOffset, position.y * boardManager.tileSize);
    }

    public override bool ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        Debug.Log("Game over");
        return true;
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
