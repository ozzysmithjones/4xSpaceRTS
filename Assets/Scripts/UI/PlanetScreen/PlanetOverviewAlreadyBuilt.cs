using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlanetOverviewAlreadyBuilt : MonoBehaviour
{
    public TMP_Text BuildSpaceText;
    public PlanetOverview planetOverview;
    public BuiltStructureButton[] builtStructureButtons = new BuiltStructureButton[10];

    // Start is called before the first frame update
    void Awake()
    {
        /*
        for (int i = 0; i < builtStructureButtons.Length; i++)
        {
            builtStructureButtons[i].structureClassIndex = i;
        }
        */

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateQuantity(int classIndex)
    {

        BuiltStructureButton builtStructureButton = builtStructureButtons[classIndex];

        int quantity = planetOverview.planet.planetColony.builtStructures[classIndex];

        builtStructureButton.UpdateQuantity(quantity);

        int buildSpace = planetOverview.planet.planetColony.totalPopulation - planetOverview.planet.planetColony.totalStructures;
        BuildSpaceText.text = "BUILD SPACE = " + buildSpace;
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
        int buildSpace = planetOverview.planet.planetColony.totalPopulation - planetOverview.planet.planetColony.totalStructures;
        BuildSpaceText.text = "BUILD SPACE = " + buildSpace;
    }
}
