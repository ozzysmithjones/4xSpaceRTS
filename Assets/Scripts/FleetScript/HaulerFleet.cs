using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HaulerState { AT_MINED_STAR, CARRYING_GOODS, AT_COLONY, RETURNING, ESCAPING};



public class HaulerFleet : Navigator
{
    public List<int> tradeRoute;
    public StarHauling starHauling;
    //public Star minedStar;
    private bool finishedPath = false;

    public HaulerState haulerState = HaulerState.AT_MINED_STAR;

    private enum LoadState { NONE,LOADING,UNLOADING};
    private LoadState loadState = LoadState.NONE;

    public override void OnFinishPath()
    {
        base.OnFinishPath();
        finishedPath = true;

        //starHauling.RequestReturn();

        
        
    }

    List<int> ReversedPath(List<int> path)
    {
        List<int> reversedPath = new List<int>(path);
        reversedPath.Reverse();
        return reversedPath;
    }

    public override void OnFinishIdle()
    {
        base.OnFinishIdle();

        switch (haulerState)
        {
            case HaulerState.CARRYING_GOODS:
                haulerState = HaulerState.AT_COLONY;
                StartCoroutine(UnLoadToNearbyPlanet(navigatorWarp.star,ReversedPath(tradeRoute)));
                break;
            case HaulerState.RETURNING:
                haulerState = HaulerState.AT_MINED_STAR;
                StartCoroutine(Load(navigatorWarp.star, tradeRoute));
                break;

        }

    }

    
    private void UnLoad()
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].GiveCargo(faction);
        }

        
    }

    private IEnumerator WaitForDroneReturn()
    {
        //wait until all the miners have returned.
        bool returned = false;
        while (!returned)
        {
            bool collected = true;
            for (int i = 0; i < spaceShips.Count; i++)
            {
                ShipCarrier shipCarrier = spaceShips[i] as ShipCarrier;
                if (shipCarrier.unDockedDrones > 0)
                {
                    collected = false;
                    break;
                }
            }
            if (!collected)
            {

                yield return new WaitForSeconds(3f);
            }
            else
            {
                returned = true;
            }
        }

    }
    

    public IEnumerator Load(Star star, List<int> path)
    {
        loadState = LoadState.LOADING;
        //send out all the miners to the planets.
        List<Transform> planets = new List<Transform>();

        for(int i = 0; i < star.starGeneration.planets.Length; i++)
        {
            if(star.starGeneration.planets  == null)
            {
                Debug.LogError("Planet array is null");
            }else if (star.starGeneration.planets[i] == null)
            {
                Debug.LogError("Planet in the array is null");
            }
            

            planets.Add(star.starGeneration.planets[i].transform);
        }

        for(int i = 0; i < spaceShips.Count; i++)
        {
            ShipCarrier shipCarrier = spaceShips[i] as ShipCarrier;
            shipCarrier.SetDronePaths(planets);
        }

        yield return WaitForDroneReturn();
        //when all the miners have returned, move on.
        SetPath(path);
        haulerState = HaulerState.CARRYING_GOODS;
        loadState = LoadState.NONE;

    }

    
    public IEnumerator UnLoadToNearbyPlanet(Star star, List<int> path)
    {
        loadState = LoadState.UNLOADING;
        for (int i = 0; i < spaceShips.Count; i++)
        {

            ShipCarrier shipCarrier = spaceShips[i] as ShipCarrier;
            shipCarrier.SetDronesPath(star.starGeneration.planets[0].transform);
        }

        yield return WaitForDroneReturn();

        UnLoad();
        finishedPath = false;
        SetPath(path);
        haulerState = HaulerState.RETURNING;
        loadState = LoadState.NONE;
    }

    



   

 




}
