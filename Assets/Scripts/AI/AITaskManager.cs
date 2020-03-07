using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskManager
{
    private AI ai;

    public AITaskManager(AI ai)
    {
        this.ai = ai;
    }

    public virtual float TaskWorth()
    {
        return 0.0f;
    }

    public virtual void DoTask()
    {

    }
}
