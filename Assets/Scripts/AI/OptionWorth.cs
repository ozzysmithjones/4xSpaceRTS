using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for any decision the AI could choose, a value is calculated representing how strong the pick would be.
public class OptionWorth : ScriptableObject
{

    public float value = 0.0f;

    public virtual float Calculate(AI ai)
    {
        return value;
    }
}
