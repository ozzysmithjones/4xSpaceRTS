using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consideration : ScriptableObject
{
    public float weight;
    public abstract float Calculate(Analysis analysis, Target target = null);

    public Consideration Clone()
    {
        Consideration copy = CreateCopy();
        copy.weight = this.weight;
        return copy;
    }

    protected abstract Consideration CreateCopy();
}
