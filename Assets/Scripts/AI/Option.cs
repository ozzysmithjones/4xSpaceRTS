using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Option",menuName = "AI/AI option")]
public class Option : ScriptableObject
{
    public Weight weight;

    //the Option class will include variables related to that option. (e.g building a ship will include the class of ship that is being built).
    //every option has a weight, the higher the value the more likely the AI will pick that option.(weight is a scriptable object so other scripts could effect the weight without needing a reference to this option).

    //regardless, there is a calculate function which can be used to calculate the value for the weight to start of with. 
    public virtual float Calculate(int empire)
    {


        return weight.values[empire];
    }
  
}
