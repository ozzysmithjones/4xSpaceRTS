using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream:Assets/Scripts/AI/OptionWorth.cs
//for any decision the AI could choose, a value is calculated representing how strong the pick would be.
public class OptionWorth : ScriptableObject
=======
[System.Serializable]
public class Worth
>>>>>>> Stashed changes:Assets/Scripts/AI/Worth.cs
{
    public Weight weight;
    Option option;
    public Worth(Option option)
    {
        this.option = option;
    }

<<<<<<< Updated upstream:Assets/Scripts/AI/OptionWorth.cs
    public float value = 0.0f;

    public virtual float Calculate(AI ai)
=======
    public float Calculate(int empire)
>>>>>>> Stashed changes:Assets/Scripts/AI/Worth.cs
    {
        return weight.values[empire];
    }
}
