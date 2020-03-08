using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubManager : Manager
{

    public SubManager(AI ai) : base(ai)
    {

    }

    public virtual float Worth()
    {
        return 0.0f;
    }
    public virtual void Manage(float deltaTime)
    {

    }
}
