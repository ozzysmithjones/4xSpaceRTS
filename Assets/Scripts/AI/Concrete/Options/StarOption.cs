using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(fileName = "AIModuleOption", menuName = "AI/Options/Star")]
public class StarOption : Option
{
    protected override Option CreateCopy()
    {
        return ScriptableObject.CreateInstance<StarOption>();
    }

    public void Expand(Star star,Empire empire)
    {
        star.TakeOver(empire);
    }
}
