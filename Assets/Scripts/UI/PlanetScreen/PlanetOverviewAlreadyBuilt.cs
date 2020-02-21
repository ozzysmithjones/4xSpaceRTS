using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOverviewAlreadyBuilt : MonoBehaviour
{
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
    }

    public void UpdateAllQuantities()
    {
        for(int i = 0; i < builtStructureButtons.Length; i++)
        {
            UpdateQuantity(i);
        }
    }
}
