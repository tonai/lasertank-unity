using System.Threading.Tasks;

public class Enemy : Block
{
    public Task CheckShoot(Direction direction)
    {
        if (direction == DirectionHelper.GetOppositeDirection(this.direction)) {
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
}
