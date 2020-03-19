using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubManager : Manager
{
    public float value;
    public SubManager(AI ai) : base(ai)
    {

    }

    public virtual float CalculateValue()
    {
        return 0.0f;
    }
    public virtual void Manage(float deltaTime)
    {

    }
}
