using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrder : IOrder
{
    private readonly Star star;
    private readonly Transform point;
    private readonly List<Star> preferedPath;

    public MoveOrder(Star star, Transform point, List<Star> preferedPath = null)
    {
        this.star = star;
        this.point = point;
        this.preferedPath = preferedPath;
    }

    public Star TargetStar => star;

    public Transform TargetPoint => point;

    public List<Star> PreferedPath => preferedPath;

    public void OnReachPoint(Fleet fleet, Transform point)
    {
    }

    public void OnReachStar(Fleet fleet, Star star)
    {
    }
}

public class ColoniseOrder : IOrder
{
    private readonly Star star;
    private readonly Planet planet;
    private readonly List<Star> preferedPath;

    public ColoniseOrder(Star star, Planet planet, List<Star> preferedPath = null)
    {
        this.star = star;
        this.planet = planet;
        this.preferedPath = preferedPath;
    }

    public Star TargetStar => star;

    public Transform TargetPoint => planet.transform;

    public List<Star> PreferedPath => preferedPath;

    public void OnReachPoint(Fleet fleet, Transform point)
    {
        for(int i = 0; i < fleet.spaceShips.Count; ++i)
        {
            if(fleet.spaceShips[i] is ColonyShip colonyShip)
            {
                colonyShip.ColonisePlanet(planet);
                return;
            }
        }
    }

    public void OnReachStar(Fleet fleet, Star star)
    {

    }
}