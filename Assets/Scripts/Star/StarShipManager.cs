using System.Collections.Generic;
using UnityEngine;

public class StarShipManager : MonoBehaviour
{
    //for later use.
    //public Dictionary<int, List<Navigator>> fleetsByFaction = new Dictionary<int,List<Navigator>>();
    public List<Fleet> fleets = new List<Fleet>();
    public Transform visuals;

    public IconHandler iconHandler;
    private Star star;

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

    //faction will be needed when there is multiple factions in one system.
    public void Remove(Fleet fleet)
    {
        if (fleet.empire == Empire.player)
        {
            fleet.Scout(false);
        }

        for (int i = 0; i < fleets.Count; i++)
        {
            fleets[i].RemoveEnemyFleet(fleet);
        }

        iconHandler.RemoveIcon(fleet.iconHandlerID);
        fleets.Remove(fleet);
        UpdateFaction();
    }

    public void Add(Fleet fleet)
    {
        if (fleet.empire == Empire.player)
        {
            fleet.Scout(true);
        }

        iconHandler.AddIcon(fleet, fleet.iconSprite, fleet.military, fleet.empire);
        fleet.transform.SetParent(visuals);
        fleet.UpdateVisibility();

        //combat check:

        for(int i = 0; i < fleets.Count; ++i)
        {
            if(fleet.empire.IsEnemyTo(fleets[i].empire))
            {
                fleet.AddEnemyFleet(fleets[i]);
                fleets[i].AddEnemyFleet(fleet);
            }
        }

        fleets.Add(fleet);
        UpdateFaction();
        return;
    }

    public int GetSmallestFleet(Empire empire, bool military = true)
    {
        if (fleets.Count <= 0)
        {
            return -1;
        }

        int index = -1;
        int size = 0;
        bool first = true;

        for (int i = 0; i < fleets.Count; i++)
        {
            //this works:
            if (fleets[i].military != military)
            {
                // Debug.Log("Military is :" + military + " but this fleet is not");
                continue;
            }

            if (fleets[i].empire == empire || empire == null)
            {

                if (first || fleets[i].spaceShips.Count < size)
                {
                    first = false;
                    size = fleets[i].spaceShips.Count;
                    index = i;
                }
            }
        }


        return index;

    }

    void UpdateFaction()
    {
        if (fleets.Count <= 0 || star.empire == null)
        {
            return;
        }

        Empire remainingEmpire = fleets[0].empire;
        for (int i = 0; i < fleets.Count; i++)
        {
            if (fleets[i].empire == star.empire)
            {
                return;
            }
            if (fleets[i].empire != remainingEmpire)
            {
                remainingEmpire = null;
            }

        }
        if (star.empire == remainingEmpire)
        {
            return;
        }
        star.TakeOver(remainingEmpire);
    }
}
