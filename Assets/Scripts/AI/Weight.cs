using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI weight",menuName ="AI/Weights")]
public class Weight : ScriptableObject
{
    public float[] values;

    public void Initialise(int empireCount)
    {
        values = new float[empireCount];
       // Debug.Log("Initialised " + this.name + " with a size of " + values.Length);
    }
}
