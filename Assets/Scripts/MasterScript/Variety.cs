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
     
        for(int i = 0; i < builtStructures.Length; i++)
        {
            builtStructures[i].classIndex = i;
        }

        for(int i = 0; i < builtShips.Length; i++)
        {
            builtShips[i].classIndex = i;
        }
    }

    
}
