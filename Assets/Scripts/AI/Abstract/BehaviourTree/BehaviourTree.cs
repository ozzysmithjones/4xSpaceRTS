using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourState
{
    Success,
    Failure,
    Running,
}

public abstract class BehaviourTreeNode : ScriptableObject
{
    public abstract void Start();
    public abstract BehaviourState Run(Analysis analysis, ref BehaviourTreeNode child);
    public abstract BehaviourState OnChildFinish(BehaviourTreeNode child, BehaviourState childState);
    public abstract BehaviourTreeNode Clone();
}

[System.Serializable]
public class BehaviourTree 
{
    public BehaviourTreeNode root;
    private readonly Stack<BehaviourTreeNode> nodes = new Stack<BehaviourTreeNode>();

    public void Reset()
    {
        nodes.Clear();
    }

    public void Run(Analysis analysis)
    {
        if(nodes.Count == 0)
        {
            root.Start();
            nodes.Push(root);
        }

        BehaviourTreeNode child = null;
        BehaviourState state;

        while((state = nodes.Peek().Run(analysis, ref child)) != BehaviourState.Running && child == null)
        {
            if(child != null)
            {
                child.Start();
                nodes.Push(child);
                child = null;
            }
            else
            {
                BehaviourTreeNode current = nodes.Pop();
                while (nodes.Count > 0 && (state = nodes.Peek().OnChildFinish(current, state)) != BehaviourState.Running)
                {
                    nodes.Pop();
                }
            }
        }
    }
}
