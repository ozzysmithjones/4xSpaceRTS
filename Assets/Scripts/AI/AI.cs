using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Faction
{
   private EconomyAI economyAI;
   public AI(int index, Color flagColor, string factionName) : base(index,flagColor,factionName)
   {
        economyAI = new EconomyAI(this);
   }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        economyAI.Update(deltaTime);

    }

    protected Weight LoadWeight(string path)
    {
        return UnityEngine.Resources.Load<Weight>(path);
    }

}
