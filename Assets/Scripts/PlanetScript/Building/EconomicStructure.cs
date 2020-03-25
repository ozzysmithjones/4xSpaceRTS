using UnityEngine;

[CreateAssetMenu(fileName = "Economic Structure", menuName = "Economy/EconomicStructure")]
public class EconomicStructure : BuiltStructure
{

    public ResourceType resourceType = ResourceType.MATERIALS;
    public int resourceProduction = 1;

    public override void OnBuild()
    {
        base.OnBuild();
        planetBuiltOn.planetColony.ModifyResourceProduction(resourceType, resourceProduction);
    }
    public override void OnRemove()
    {
        base.OnRemove();
        planetBuiltOn.planetColony.ModifyResourceProduction(resourceType, -resourceProduction);
    }
    public EconomicStructure(string name, string description, ResourceType resourceProduced = ResourceType.MATERIALS, int resourceQuantity = 2) : base(name, description)
    {
        this.resourceType = resourceProduced;
        this.resourceProduction = resourceQuantity;
    }
}
