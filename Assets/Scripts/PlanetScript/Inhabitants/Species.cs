using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Species 
{

    public int index = -1;
    public Color color;
    public BiomeType biomePreference;
    public int animation;
    public float[] politicalBias;

    public string name = "Alien";
    public string description = "bad";

}
