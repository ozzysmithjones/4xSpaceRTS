using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyShip : SpaceShip
{
    public void ColonisePlanet(Planet planet)
    {
        planet.star.TakeOver(fleet.empire);
        planet.Colonise(fleet.empire);
        Explode();
    }
}
