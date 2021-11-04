public class State
{
    public (int, object)[,] ground;
    public (int, object)[,] floor;
    public (int, object)[,] objects;

    public State(int x, int z)
    {
        ground = new (int, object)[x, z];
        floor = new (int, object)[x, z];
        objects = new (int, object)[x, z];
    }

    public void AddCell(int i, int j, (int, object) serializedGround, (int, object) serializedFloor, (int, object) serializedObject)
    {
        ground[i, j] = serializedGround;
        floor[i, j] = serializedFloor;
        objects[i, j] = serializedObject;
    }
}
