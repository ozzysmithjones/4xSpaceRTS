using System.Collections.Generic;
using UnityEngine;



public class StarHauling : MonoBehaviour
{
    public GameObject freighterFleetPrefab;
    private Star star;
    private HaulerFleet hauler;

    private List<int> route;

  
    

    public void Initialise()
    {
        star = GetComponent<Star>();
        Navigator freighter = star.starConstruction.Build(freighterFleetPrefab, StarConstruction.StarConstructionType.fleet);

        hauler = freighter as HaulerFleet;
        //hauler.minedStar = star;
        hauler.starHauling = this;
        hauler.tradeRoute = route;

        //disable the freighter until this place is colonised.
        
        if (freighter.star.starShipManager.RequestExit(freighter.faction, freighter))
        {
            freighter.gameObject.SetActive(false);
        }
        else
        {
            print("freighter exit not permitted");
        }
        
    }

    public void StartHauling()
    {
        //calculate route to nearest colony. 

        hauler.SetFaction(star.factionIndex);

        route = ShortestPathToNearestColony();
        hauler.tradeRoute = route;
        hauler.gameObject.SetActive(true);
        star.starShipManager.Entry(star.factionIndex, hauler);


        hauler.StartCoroutine(hauler.Load(star,route));
    }

    List<int> ShortestPathToNearestColony()
    {
        Faction faction = Master.instance.factions.factions[star.factionIndex];

        int shortest = 0;
        List<int> shortestPath = new List<int>();

        int[] factionArray = new int[1];
        factionArray[0] = star.factionIndex;

        for (int i = 0; i < faction.Colonies.Count; i++)
        {
           
            List<int> path = Master.instance.PathFind(star.position, faction.Colonies[i].position,factionArray);
            if(path.Count-1 < shortest || i == 0)
            {
                shortest = path.Count - 1;
                shortestPath = path;
            }
        }

        if(shortestPath.Count <= 1)
        {
            Debug.LogError("SHORT PATH");
        }

        return shortestPath;
    }

    public List<int> ReCalculatePath(HaulerState haulerState)
    {
        if (haulerState == HaulerState.CARRYING_GOODS || haulerState == HaulerState.AT_MINED_STAR)
        {
            haulerState = HaulerState.RETURNING;
            route = ShortestPathToNearestColony();
           
        }
        else if (haulerState == HaulerState.AT_COLONY || haulerState == HaulerState.RETURNING)
        {
            haulerState = HaulerState.CARRYING_GOODS;
            route = ShortestPathToNearestColony();
            route.Reverse();
            

        }
        return route;

    }

}
