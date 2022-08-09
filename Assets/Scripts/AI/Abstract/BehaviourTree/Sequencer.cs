using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequencer", menuName = "AI/Behaviour Tree/Sequencer")]
public class Sequencer : BehaviourTreeNode
{
    public BehaviourTreeNode[] children;
    private int index = 0;

    public override BehaviourTreeNode Clone()
    {
        Sequencer copy = ScriptableObject.CreateInstance<Sequencer>();
        copy.children = new BehaviourTreeNode[this.children.Length];
        copy.index = this.index;

        for (int i = 0; i < children.Length; ++i)
        {
            copy.children[i] = children[i].Clone();
        }

        return copy;
    }

    public override BehaviourState OnChildFinish(BehaviourTreeNode child, BehaviourState childState)
    {
        if (childState == BehaviourState.Failure)
        {
            return BehaviourState.Failure;
        }

        ++index;
        return index >= children.Length ? BehaviourState.Success : BehaviourState.Running;
    }

    public override BehaviourState Run(Analysis analysis, ref BehaviourTreeNode child)
    {
        child = children[index];
        return BehaviourState.Success;
    }

    public override void Start()
    {

    }
}
