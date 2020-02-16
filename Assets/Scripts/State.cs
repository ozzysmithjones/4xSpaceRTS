using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{

    public bool canChangeState = true;
    public bool tickEnabled = false;
    public Transition[] transitions = new Transition[0];

    public State(bool canChangeState = false,bool tickEnabled = false)
    {

    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnTick()
    {

    }

    public virtual void OnExit()
    {

    }
    
}
