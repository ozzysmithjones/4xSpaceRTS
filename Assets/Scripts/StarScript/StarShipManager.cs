using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShipManager : MonoBehaviour
{
    //for later use.
    //public Dictionary<int, List<Navigator>> fleetsByFaction = new Dictionary<int,List<Navigator>>();

    private bool conflict = false;
    public List<Navigator> fleets = new List<Navigator>();
    public Transform visuals;

    public IconHandler iconHandler;
    private Star star;

    private int opposing= 0;
    private int defending = 0;
    
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
    public bool RequestExit(Navigator navigator)
    {
        
        if (navigator.faction == 0)
        {
            navigator.Scout(false);
        }
        for (int i = 0; i < fleets.Count; i++)
        {
            fleets[i].navigatorCombat.RemoveEnemy(navigator);
        }

        iconHandler.RemoveIcon(navigator.iconHandlerID);
        fleets.Remove(navigator);
        UpdateFaction();
        return true;
    }

    public void Entry( Navigator navigator)
    {
        if (navigator.faction == 0)
        {
            navigator.Scout(true);
        }

        iconHandler.AddIcon(navigator,navigator.iconSprite,navigator.military,navigator.faction);
        navigator.transform.SetParent(visuals);
        navigator.UpdateVisibility();


        //combat check:
        navigator.navigatorCombat.IsConflict(fleets);
        for(int i = 0; i < fleets.Count; i++)
        {
            fleets[i].navigatorCombat.AddEnemy(navigator);
        }
        fleets.Add(navigator);
        UpdateFaction();
        return;
    }

    //not used:
    public void DestroyFleet(Navigator navigator)
    {
        RequestExit(navigator);
        Destroy(navigator.gameObject);
    }




    public int GetSmallestFleet(int faction = -1, bool military = true)
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
            

            if (fleets[i].faction == faction || faction == -1)
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
        if(fleets.Count <= 0 || star.factionIndex == -1)
        {
            return;
        }

        int remainingFaction = fleets[0].faction; 
        for(int i = 0; i < fleets.Count; i++)
        {
            if(fleets[i].faction == star.factionIndex)
            {
                return;
            }
            if(fleets[i].faction != remainingFaction)
            {
                remainingFaction = -1;
            }
            
        }
        if(star.factionIndex == remainingFaction)
        {
            return;
        }
        star.TakeOver(remainingFaction);
    }

}
