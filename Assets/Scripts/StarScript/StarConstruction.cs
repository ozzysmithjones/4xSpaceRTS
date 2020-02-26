using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarConstruction : MonoBehaviour
{
    private Star star;
    public GameObject emptyFleetPrefab;
    public Transform visuals;
    public enum StarConstructionType { fleet, spaceShip, spaceStation}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialise()
    {

        star = GetComponent<Star>();

    }

    public Navigator Build(GameObject prefab, StarConstruction.StarConstructionType type)
    {

        GameObject spawned = Instantiate(prefab, transform.position,transform.rotation);
        spawned.transform.SetParent(visuals);

        if(type == StarConstructionType.spaceStation)
        {
            star.starVisibility.AddStaticObject(spawned);
        }
        if(type == StarConstructionType.spaceShip)
        {
            SpaceShip spaceShip = spawned.GetComponent<SpaceShip>();
            spaceShip.Initialise(Master.instance.factions.factions[star.factionIndex].flagColor);
            int fleetIndex = star.starShipManager.GetSmallestFleet(star.factionIndex,true);

            if(fleetIndex >= 0)
            {
                star.starShipManager.fleets[fleetIndex].AddShip(spaceShip);
                spaceShip.transform.position = (Vector3)Random.insideUnitCircle * 1.5f + spaceShip.transform.position;
                return null;
            }
            else
            {
                Navigator navigator = Instantiate(emptyFleetPrefab, transform.position, transform.rotation).GetComponent<Navigator>();
                navigator.Initialise();

                navigator.AddShip(spaceShip);
                navigator.SetStar(star);
                navigator.faction = star.factionIndex;
                star.starShipManager.Entry(navigator);
                return navigator;

            }
        }
        if(type == StarConstructionType.fleet)
        {
            Navigator navigator = spawned.GetComponent<Navigator>();
            navigator.Initialise();

            navigator.SetStar(star);
            navigator.faction = star.factionIndex;
            star.starShipManager.Entry(navigator);
            return navigator;
           
        }
        return null;
    }
}
