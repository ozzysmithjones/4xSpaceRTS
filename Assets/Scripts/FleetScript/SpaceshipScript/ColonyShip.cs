using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyShip : SpaceShip
{
    public void ColonisePlanet(Planet planet)
    {
        planet.Colonise(fleet.empire);
        Explode();
    }
}
