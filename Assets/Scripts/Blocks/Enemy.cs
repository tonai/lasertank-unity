using System;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : Block
{
    protected bool isDestroyed = false;

    public Task CheckShoot(Direction direction)
    {
        if (direction == DirectionHelper.GetOppositeDirection(this.direction) && !isDestroyed) {
            Shooter shooter = GetComponent<Shooter>();
            if (shooter != null)
            {
                TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
                shooter.Shoot(GetPosition(), this.direction, () => promise.SetResult(true));
                return promise.Task;
            }
        }
        return null;
    }

    public override Vector3 GetShootHitPosition(float yOffset, ref Direction direction, ref bool continueShooting)
    {
        if (direction == DirectionHelper.GetOppositeDirection(this.direction))
        {
            continueShooting = false;
            float tileSize = BoardManager.current.tileSize;
            return new Vector3(position.x * tileSize, yOffset, position.y * tileSize);
        }

        return base.GetShootHitPosition(yOffset, ref direction, ref continueShooting);
    }

    public override (int, object) Serialize()
    {
        return (id, isDestroyed);
    }

    public override void SetState(object state)
    {
        isDestroyed = (bool)state;
        if (isDestroyed)
        {
            ApplyColor(ComponentHelper.FindComponentInChildWithTag<Transform>(gameObject, "Tank"));
        }
    }

    public override void ShootThrough(GameObject gameObject, Direction direction, Shooter shooter, Action callback)
    {
        if (!isDestroyed && direction == DirectionHelper.GetOppositeDirection(this.direction))
        {
            HistoryManager.current.Push();
            isDestroyed = true;
            ApplyColor(ComponentHelper.FindComponentInChildWithTag<Transform>(this.gameObject, "Tank"));
            shooter.DestroyLaser();
            callback();
            return;
        }

        base.ShootThrough(gameObject, direction, shooter, callback);
    }

    private void ApplyColor(Transform transform)
    {
        Renderer renderer = transform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.black;
            Transform[] t = renderer.gameObject.GetComponentsInChildren<Transform>();
            if (t.Length > 1) {
                ApplyColor(t[1]);
            }
        }
    }
}
