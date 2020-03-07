using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream:Assets/Scripts/AI/OptionWorth.cs
//for any decision the AI could choose, a value is calculated representing how strong the pick would be.
<<<<<<< HEAD:Assets/Scripts/AI/OptionWorth.cs
public class OptionWorth : ScriptableObject
=======
[System.Serializable]
public class Worth
>>>>>>> Stashed changes:Assets/Scripts/AI/Worth.cs
=======
public class Worth
>>>>>>> eef69385ad9dd06bb4d34d8e2ec82af48de9bee0:Assets/Scripts/AI/Worth.cs
{
    public Weight weight;
    Option option;
    public Worth(Option option)
    {
        this.option = option;
    }

<<<<<<< Updated upstream:Assets/Scripts/AI/OptionWorth.cs
    public float value = 0.0f;

<<<<<<< HEAD:Assets/Scripts/AI/OptionWorth.cs
    public virtual float Calculate(AI ai)
=======
    public float Calculate(int empire)
>>>>>>> Stashed changes:Assets/Scripts/AI/Worth.cs
=======
    public virtual float Calculate()
>>>>>>> eef69385ad9dd06bb4d34d8e2ec82af48de9bee0:Assets/Scripts/AI/Worth.cs
    {
        return weight.values[empire];
    }
}
