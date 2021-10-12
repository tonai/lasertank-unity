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
        bool isKeyPressed = false;

        if (Input.GetKey(shoot))
        {
            isKeyPressed = true;
            if (canPerformAction)
            {
                Direction direction = block.GetDirection();
                shooter.Shoot(block.GetPosition(), direction);
                return;
            }
        }

        if (Input.GetKey(forward))
        {
            isKeyPressed = true;
            targetDirection = Direction.North;
        }
        if (Input.GetKey(backward))
        {
            isKeyPressed = true;
            targetDirection = Direction.South;
        }
        if (Input.GetKey(right))
        {
            isKeyPressed = true;
            targetDirection = Direction.Est;
        }
        if (Input.GetKey(left))
        {
            isKeyPressed = true;
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
                HandleArrowDirectionMovement((Direction)targetDirection);
            }
        }

        movable.SetKeyDown(isKeyPressed);
        rotable.SetKeyDown(isKeyPressed);
        // movable.SetKeyDown(relativeTargetDirection == Direction.North || relativeTargetDirection == Direction.South);
        // rotable.SetKeyDown(relativeTargetDirection == Direction.Est || relativeTargetDirection == Direction.West);
    }

    private void HandleArrowDirectionMovement(Direction targetDirection)
    {
        Direction direction = block.GetDirection();

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
        Direction direction = block.GetDirection();

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
