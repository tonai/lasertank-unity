using UnityEngine;

public class Opener : MonoBehaviour
{
    private int[] keys = new int[8];

    public void AddKey(int index)
    {
        keys[index]++;
        UIManager.current.CreateOrUpdate(index, keys[index].ToString());
    }

    public bool CanOpenDoor(int index)
    {
        return keys[index] > 0;
    }

    public void OpenDoor(int index)
    {
        if (CanOpenDoor(index))
        {
            keys[index]--;
            UIManager.current.UpdateUI(index, keys[index].ToString());
        }
    }

    public int[] Serialize()
    {
        return (int[])keys.Clone();
    }

    public void SetState(int[] state)
    {
        for (int i = 0; i < state.Length; i++) {
            keys[i] = state[i];
            UIManager.current.UpdateUI(i, keys[i].ToString());
        }
    }

    
}
