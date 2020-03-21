using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//for speciesIndex:
// 0 = empire species.
// 1 = robots(if any).

public class Population 
{
    public Species species;
    public int size;
    public float happyness;

    public Population(Species speciesIndex, int size, float happyness)
    {
        this.species = speciesIndex;
        this.size = size;
        this.happyness = happyness;
    }

    public float CalculateHappyness(float[] politicalStance)
    {
        float value = 0.0f;
        float max = 0.0f;
        for(int i = 0; i < species.politicalBias.Length; i++)
        {
            value += species.politicalBias[i] * politicalStance[i];
            max += species.politicalBias[i];
        }
        happyness = value / max;
        return happyness;
    }
}
