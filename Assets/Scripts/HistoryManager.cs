using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    public static HistoryManager current;

    public KeyCode rewind;

    private State lastState;
    private Stack<State> states = new Stack<State>();

    public void Update()
    {
        if (Input.GetKeyDown(rewind))
        {
            Rewind();
        }
    }

    private void Awake()
    {
        current = this;
    }

    public void Push()
    {
        State state = Serialize();
        SaveState(state);
    }

    public void Rewind()
    {
        if (states.Count == 0)
        {
            this.lastState = null;
            return;
        }

        State state = states.Pop();
        BoardManager.current.Reset(state);

        if (states.Count == 0)
        {
            this.lastState = null;
            return;
        }

        State lastState = states.Pop();
        this.lastState = lastState;
        states.Push(lastState);
    }

    public void SaveState(State state)
    {
        states.Push(state);
        lastState = state;
    }

    public State Serialize()
    {
        Board board = BoardManager.current.GetBoard();
        int xSize = board.GetXSize();
        int zSize = board.GetZSize();
        State state = new State(xSize, zSize);

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                (int, object) serializedGround = board.ground[i].Serialize(j);
                (int, object) serializedFloor = board.floor[i].Serialize(j);
                (int, object) serializedObject = board.objects[i].Serialize(j);
                state.AddCell(i, j, serializedGround, serializedFloor, serializedObject);
            }
        }

        return state;
    }
}
