using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class StaticObject : ScriptableObject
{

    [SerializeField]
    public List<GameObject> staticObjects = new List<GameObject>();

}