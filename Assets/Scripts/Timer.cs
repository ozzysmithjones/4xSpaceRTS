using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//does a thing at an interval.Very common behaviour, so this should clean code up quite a lot.
public class Timer
{
    public bool looping;
    private bool finished = false;
    private float timePassed;
    public float endTime;
    public delegate void Effect();
    private Effect effect;

    public Timer(float _endTime, Effect _effect, bool _looping = true)
    {
        endTime = _endTime;
        effect = _effect;
        looping = _looping;
    }

    public bool Tick(float deltaTime)
    {
        if (finished)
        {
            return true;
        }
        timePassed += deltaTime;
        if(timePassed > endTime)
        {
            effect();
            timePassed = 0;
            if (!looping)
            {
                finished = true;
            }

            return true;
        }

        return false;
    }

    public void ChangeEffect(Effect _effect)
    {
        effect = _effect;
    }

}
