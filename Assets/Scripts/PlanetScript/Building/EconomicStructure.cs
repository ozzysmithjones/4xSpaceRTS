using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Economic Structure", menuName = "Economy/EconomicStructure")]
public class EconomicStructure : BuiltStructure
{
   
    public ResourceType resourceType = ResourceType.MATERIALS;
    public int resourceproduction = 1;


    public override void OnBuild()
    {
        base.OnBuild();

        planetBuiltOn.ImproveResourceproduction(resourceType, resourceproduction);
    }

    public override void OnRemove()
    {
        base.OnRemove();

        planetBuiltOn.ImproveResourceproduction(resourceType, -resourceproduction);

    }
    public EconomicStructure(string name, string description, ResourceType resourceProduced = ResourceType.MATERIALS, int resourceQuantity = 2) : base(name, description)
    {
        this.resourceType = resourceProduced;
        this.resourceproduction = resourceQuantity;


    }




}
