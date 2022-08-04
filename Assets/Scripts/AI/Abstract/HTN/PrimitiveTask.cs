using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrimitiveTask", menuName = "AI/HTN/Primitive Task")]
public class PrimitiveTask : Task
{
    [SerializeField] private List<Condition> effects = new List<Condition>();

    public void Apply(GameState gameState)
    {
        for (int i = 0; i < effects.Count; ++i)
        {
            gameState.Set(effects[i].proposition, effects[i].value);
        }
    }
}
