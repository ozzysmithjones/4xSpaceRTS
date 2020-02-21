using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variety : MonoBehaviour
{
    //this script simply stores arrays of different structure types, ship types ect.
    //this is used on the colony building process, where the correct building is built by referencing the index of the item.

    public BuiltStructure[] builtStructures = new BuiltStructure[10];
    public BuiltShip[] builtShips = new BuiltShip[1];



    public void Initialise()
    {
        builtStructures[0] = new EconomicStructure("Mine","produces 2 materials every minute", ResourceType.MATERIALS, 2);
        builtStructures[1] = new EconomicStructure("Generator","produces two energy every minute",ResourceType.ENERGY,2);
        builtStructures[2] = new EconomicStructure("Chemical plant", "produces 1 death matter every minute",ResourceType.DEATHMATTER,1);




        for(int i = 0; i < builtStructures.Length; i++)
        {
            if(builtStructures[i] == null)
            {
                builtStructures[i] = new BuiltStructure("Ruin","");
            }
            builtStructures[i].classIndex = i;
        }

        for(int i = 0; i < builtShips.Length; i++)
        {
            builtShips[i].classIndex = i;
        }
    }

    
}
