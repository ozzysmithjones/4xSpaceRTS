using System.Collections;
using System.Collections.Generic;
using UnityEngine;




enum OpType
{
    Additive,
    Multiplicative,
}

public abstract class Option : ScriptableObject
{
    [SerializeField] private float weight = 1.0f;
    [SerializeField] private OpType operationType = OpType.Multiplicative;
    [SerializeField] private List<Consideration> considerations = new List<Consideration>();

    public Option Clone()
    {
        Option copy = CreateCopy();
        copy.weight = this.weight;
        copy.operationType = this.operationType;
        copy.considerations.Clear();

        foreach(Consideration consideration in considerations)
        {
            copy.considerations.Add(consideration.Clone());
        }

        return copy;
    }

    public float Calculate(Target target, Analysis analysis)
    {
        float value = 1.0f;

        if (operationType == OpType.Multiplicative)
        {
            foreach (Consideration consideration in considerations)
            {
                value *= consideration.Calculate(analysis, target) * consideration.weight;
            }

        }else
        {
            foreach (Consideration consideration in considerations)
            { 
                value += consideration.Calculate(analysis, target) * consideration.weight;
            }
        }    

        value *= weight;
        return value;
    }

    protected abstract Option CreateCopy();
}
