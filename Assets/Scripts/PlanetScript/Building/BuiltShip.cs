using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(fileName = "Ship", menuName = "Economy/Ship")]
public class BuiltShip : BuildQueueItem
{
    public GameObject prefab;
    public StarConstruction.StarConstructionType type = StarConstruction.StarConstructionType.spaceShip;

    public override void Build(Planet planet)
    {

        base.Build(planet);
        planet.star.starConstruction.Build(prefab, type);
    }

    public BuiltShip(string name, string description) : base(name, description)
    {



    }


}
