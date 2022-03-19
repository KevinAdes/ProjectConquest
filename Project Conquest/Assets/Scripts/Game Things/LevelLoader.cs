using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    Vector3 MapCoordinates;
    [SerializeField]
    string ID;
    [SerializeField]
    bool right;

    //there should be no reason to set the ID or Right outside of the editor, so I will not be writing setters for them

    public string GetID()
    {
        return ID;
    }

    public bool GetRight()
    {
        return right;
    }
    public Vector3 GetCoords()
    {
        return MapCoordinates;
    }
}
