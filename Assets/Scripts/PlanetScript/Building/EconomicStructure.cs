using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomicStructure : BuiltStructure
{
   
    public ResourceType resourceType = ResourceType.MATERIALS;
    private int resourceproduction = 1;


    public override void OnBuild()
    {
        base.OnBuild();

        planetBuiltOn.planetColony.resourceProduction.amounts[(int)resourceType] += resourceproduction;
    }

    public override void OnRemove()
    {
        base.OnRemove();

        planetBuiltOn.planetColony.resourceProduction.amounts[(int)resourceType] -= resourceproduction;

    }
    public EconomicStructure(string name, string description, ResourceType resourceProduced = ResourceType.MATERIALS, int resourceQuantity = 2) : base(name, description)
    {
        this.resourceType = resourceProduced;
        this.resourceproduction = resourceQuantity;


    }




}
