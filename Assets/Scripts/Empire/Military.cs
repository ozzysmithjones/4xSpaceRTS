using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public enum FleetType
{
    Exploration,
    Colony,
    Military,
}

public class Military
{
    private Empire empire;
    private readonly List<Fleet>[] fleetsByType;
    private readonly List<Empire> enemies = new List<Empire>();

    public Military(Empire empire)
    {
        this.empire = empire;
        this.fleetsByType = new List<Fleet>[Enum.GetValues(typeof(FleetType)).Length];

        if (this.empire != Empire.player)
        {
            this.enemies.Add(Empire.player);
        }

        for(int i = 0; i < fleetsByType.Length; ++i)
        {
            fleetsByType[i] = new List<Fleet>();
        }
    }

    public List<Empire> GetEnemies()
    {
        return enemies;
    }

    public void AddEnemy(Empire empire)
    {
        enemies.Add(empire);
    }

    public void RemoveEnemy(Empire empire)
    {
        enemies.Remove(empire);
    }


    public List<Fleet> GetFleets(FleetType fleetType)
    {
        return fleetsByType[(int)fleetType];
    }

    public void RemoveFleet(Fleet fleet)
    {
        List<Fleet> fleets = fleetsByType[(int)fleet.type];
        int index = fleets.IndexOf(fleet);

        if (index >= 0)
        {
            fleets[index] = fleets[fleets.Count - 1];
            fleets.RemoveAt(fleets.Count - 1);
            fleet.empire = null;
        }
    }

    public void AddFleet(Fleet fleet)
    {
        if(fleet.empire != null)
        {
            fleet.empire.military.RemoveFleet(fleet);
        }

        fleetsByType[(int)fleet.type].Add(fleet);
        fleet.empire = empire;
    }
}
