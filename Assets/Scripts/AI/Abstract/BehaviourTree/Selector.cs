using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selector", menuName = "AI/Behaviour Tree/Selector")]
public class Selector : BehaviourTreeNode
{
    public BehaviourTreeOption[] options;

    public override BehaviourTreeNode Clone()
    {
        Selector copy = ScriptableObject.CreateInstance<Selector>();
        copy.options = new BehaviourTreeOption[this.options.Length];

        for (int i = 0; i < options.Length; ++i)
        {
            copy.options[i] = (BehaviourTreeOption)options[i].Clone();
        }

        return copy;
    }

    public override BehaviourState OnChildFinish(BehaviourTreeNode child, BehaviourState childState)
    {
        return childState;
    }

    public override BehaviourState Run(Analysis analysis, ref BehaviourTreeNode child)
    {
        //Select an option

        child = options[0].node;
        float maxUtility = float.MinValue;
        for (int i = 0; i < options.Length; ++i)
        {
            float utility = options[i].Calculate(null, analysis);
            if (utility > maxUtility)
            {
                maxUtility = utility;
                child = options[i].node;
            }
        }

        return BehaviourState.Success;
    }

    public override void Start()
    {

    }
}
