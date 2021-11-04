using System;
using System.Collections.Generic;
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

    public override void Awake()
    {
        base.Awake();
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
        float tileSize = BoardManager.current.tileSize;
        return new Vector3(position.x * tileSize, yOffset, position.y * tileSize);
    }

    public override (int, object) Serialize()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Direction", direction);

        Opener opener = GetComponent<Opener>();
        if (opener != null)
        {
            data.Add("Opener", opener.Serialize());
        }
        
        return (id, data);
    }

    public override void SetState(object state) {
        Dictionary<string, object> data = (Dictionary<string, object>)state;

        if (data["Direction"] != null)
        {
            direction = (Direction)data["Direction"];
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, DirectionHelper.GetRotation(direction) + rotationOffset, transform.rotation.eulerAngles.z);
        }

        Opener opener = GetComponent<Opener>();
        if (opener != null && data["Opener"] != null)
        {
            opener.SetState((int[])data["Opener"]);
        }
    }

    public override void ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        Block block = gameObject.GetComponent<Block>();
        if (block != null && block.id == 100) {
            HistoryManager.current.Push();
        }
        shooter.DestroyLaser();
        Debug.Log("Game over");
    }

    private void HandleRelativeMovement(Direction targetDirection)
    {
        Direction direction = block.direction;

        switch (targetDirection)
        {
            case Direction.North:
                movable.Move(direction, true);
                break;

            case Direction.South:
                movable.Move(DirectionHelper.GetOppositeDirection(direction), true);
                break;

            case Direction.Est:
                rotable.Rotate(90f, true);
                break;

            case Direction.West:
                rotable.Rotate(-90f, true);
                break;

        }
    }

    private void HandleCardinalPointsMovement(Direction targetDirection)
    {
        Direction direction = block.direction;

        if (direction == targetDirection)
        {
            movable.Move(direction, true);
        }
        else
        {
            rotable.Rotate(DirectionHelper.GetRotation(direction, targetDirection, transform.rotation.eulerAngles.y), true);
        }
    }
}
