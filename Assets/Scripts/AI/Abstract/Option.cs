using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Consideration : ScriptableObject
{
    public float weight;

    public abstract float Calculate(Analysis analysis);
}


enum OpType
{
    Additive,
    Multiplicative,
}

public class Option : ScriptableObject
{
    [SerializeField] private float weight = 1.0f;
    [SerializeField] private OpType operationType = OpType.Multiplicative;
    [SerializeField] private List<Consideration> considerations = new List<Consideration>();

    public float Calculate(Analysis analysis)
    {
        float value = 1.0f;

        if (operationType == OpType.Multiplicative)
        {
            foreach (Consideration consideration in considerations)
            {
                value *= consideration.Calculate(analysis) * consideration.weight;
            }

        }else
        {
            foreach (Consideration consideration in considerations)
            { 
                value += consideration.Calculate(analysis) * consideration.weight;
            }
        }    

        value *= weight;
        return value;
    }
}
