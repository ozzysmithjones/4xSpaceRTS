using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuiltStructure : BuildQueueItem
{
    public Planet planetBuiltOn;
    public override void Build(Planet planet)
    {
        base.Build(planet);
        planetBuiltOn = planet;
        planetBuiltOn.planetColony.AddStructure(this);
        OnBuild();
    }

   

    public virtual void OnBuild()
    {

    }
    public virtual void OnRemove()
    {

    }

    public BuiltStructure (string name, string description) : base(name,description){



    }

    
}
