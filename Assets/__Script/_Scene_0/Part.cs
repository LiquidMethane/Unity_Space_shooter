using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part 
{
    public string name; //name of the part
    public float health; //health of the part
    [HideInInspector]
    public GameObject go;

}
