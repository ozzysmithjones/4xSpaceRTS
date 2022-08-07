using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inverter", menuName = "AI/Behaviour Tree/Inverter")]
public class Inverter : BehaviourTreeNode
{
    public BehaviourTreeNode child;

    public override BehaviourTreeNode Clone()
    {
        Inverter copy = CreateInstance<Inverter>();
        copy.child = this.child.Clone();
        return copy;
    }

    public override BehaviourState OnChildFinish(BehaviourTreeNode child, BehaviourState childState)
    {
        return (BehaviourState)(~((int)childState));
    }

    public override BehaviourState Run(Analysis analysis, ref BehaviourTreeNode child)
    {
        child = this.child;
        return BehaviourState.Success;
    }

    public override void Start()
    {

    }
}
