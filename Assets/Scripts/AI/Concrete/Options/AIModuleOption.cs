using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AIModuleOption", menuName = "AI/Options/AI Module")]
public class AIModuleOption : Option
{
    public AIModule module;

    public void Behave(Analysis analysis)
    {
        module.Behave(analysis);
    }

    protected override Option CreateCopy()
    {
        AIModuleOption copy = ScriptableObject.CreateInstance<AIModuleOption>();
        copy.module = this.module.Clone();
        return copy;
    }
}
