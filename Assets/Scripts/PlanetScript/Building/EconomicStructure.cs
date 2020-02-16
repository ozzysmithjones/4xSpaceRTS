using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomicStructure : BuiltStructure
{
   
    public int resourceProduced = 1;
    public int resourceQuantity = 2;


    public override void OnBuild()
    {
        base.OnBuild();

        planetBuiltOn.planetColony.production[resourceProduced] += resourceQuantity;
    }

    public override void OnRemove()
    {
        base.OnRemove();

        planetBuiltOn.planetColony.production[resourceProduced] -= resourceQuantity;

    }
    public EconomicStructure(string name, string description, int resourceProduced = 1, int resourceQuantity = 2) : base(name, description)
    {
        this.resourceProduced = resourceProduced;
        this.resourceQuantity = resourceQuantity;


    }




}
