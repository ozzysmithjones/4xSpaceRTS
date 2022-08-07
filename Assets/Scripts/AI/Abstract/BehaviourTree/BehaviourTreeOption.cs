using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BehaviourOption", menuName = "AI/Behaviour Tree/Option")]
public class BehaviourTreeOption : Option
{
    public BehaviourTreeNode node;

    protected override Option CreateCopy()
    {
        BehaviourTreeOption copy = ScriptableObject.CreateInstance<BehaviourTreeOption>();
        copy.node = node.Clone();
        return copy;
    }
}
