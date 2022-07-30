using System.Collections.Generic;
using UnityEngine;

public class StarEconomy : MonoBehaviour
{
    public List<PlanetColony> colonies = new List<PlanetColony>();
    public Resources totalResourceProduction = new Resources();
    private Star star;

    public void StartEconomy()
    {

    }

    public void Initialise()
    {
        star = GetComponent<Star>();
    }

    public void ApplyResourceproduction(bool positive)
    {
        if (star.empire == null)
        {
            return;
        }

        star.empire.economy.AddProduction(positive ? totalResourceProduction : -totalResourceProduction);
    }
}
