using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlanetOverviewAlreadyBuilt : MonoBehaviour
{
    public ToolTip productionToolTip;
    public TMP_Text productivityText;
    public PlanetOverview planetOverview;
    public BuiltStructureButton[] builtStructureButtons = new BuiltStructureButton[10];

    public void UpdateQuantity(int classIndex)
    {

        BuiltStructureButton builtStructureButton = builtStructureButtons[classIndex];
        int quantity = planetOverview.planet.planetColony.builtStructures[classIndex];

        builtStructureButton.UpdateQuantity(quantity);
        UpdateProductionUI();
    }

    public void UpdateAllQuantities()
    {
        for (int i = 0; i < builtStructureButtons.Length; i++)
        {
            UpdateQuantity(i);
        }
    }

    public void OnPopulationChange(List<Population> populations)
    {
        UpdateProductionUI();
    }

    private void UpdateProductionUI()
    {
 
        int totalPopulation = planetOverview.planet.planetColony.totalPopulation;
        int totalStructures = planetOverview.planet.planetColony.totalStructures;

        int efficiency = totalStructures > 0 ? Mathf.FloorToInt((float)totalPopulation / (float)totalStructures * 100) : 100;
        string enoughPops = totalStructures <= totalPopulation ? " Beacuse There is enough Population for every job" : " Beacuse there's not enough population for every job.";

        productivityText.text = totalPopulation + " pops : " + totalStructures + " jobs";
        productionToolTip.SetText("This planet is working at " + efficiency + " % Efficiency" + enoughPops);
    }
}
