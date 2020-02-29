using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BiomeGradient
{

    public Biome[] biomes;

    public Biome GetBiomeAtPoint(float point)
    {
        int index = (int)((float)(biomes.Length-1) * point);
        return biomes[index];

    }

   
}
