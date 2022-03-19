using UnityEngine;

[CreateAssetMenu(fileName = "Integer", menuName = "ScriptableObjects/Integer", order = 1)]
public class IntStorage : ScriptableObject
{
    [SerializeField]
    int i;

    public int GetInt()
    {
        return i;
    }
    public void SetInt(int j)
    {
        i = j;
    }
}
