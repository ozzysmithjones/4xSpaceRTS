using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomy : Economy
{
    private Text[] resourcesText;

    public override void Init(Territory territory)
    {
        base.Init(territory);
        this.resourcesText = Master.instance.userInterface.resourcesText;
    }

    public override void AddProduction(Resources production)
    {
        base.AddProduction(production);
        UpdateText();
    }

    public override void AddResources(Resources resources)
    {
        base.AddResources(resources);
        UpdateText();
    }

    public override void SetResourceAmount(ResourceType resourceType, int amount)
    {
        base.SetResourceAmount(resourceType, amount);
        UpdateText();
    }

    private void UpdateText()
    {
        for (int i = 0; i < this.resources.amounts.Length; ++i)
        {
            resourcesText[i].text = ResourceText((ResourceType)i);
        }
    }

    private string ResourceText(ResourceType resourceType)
    {
        float amount = production.amounts[(int)resourceType];
        string productionText = amount >= 0 ? "+" + amount : "-" + -amount;
        return resourceType.ToString() + ": " + this.resources.amounts[(int)resourceType] + " " + productionText;
    }

   
}
