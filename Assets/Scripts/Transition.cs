using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition 
{

    public State state;
    public virtual bool ShouldChangeState()
    {

        return true;
    }

}
