using UnityEngine;

public class LockManager : ScriptableObject
{
    GameObject door;
    int ID;
    bool closed;

    public void SetGuy(GameObject guy)
    {
        door = guy;
    }
    public GameObject GetGuy()
    {
        return door;
    }

    public void SetID(int i)
    {
        ID = i;
    }
    public int GetID()
    {
        return ID;
    }
    public void SetClosed(bool b)
    {
        closed = b;
    }
    public bool GetClosed()
    {
        return closed;
    }
}
