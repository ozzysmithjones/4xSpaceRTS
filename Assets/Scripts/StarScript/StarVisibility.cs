using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StarShipManager))]
public class StarVisibility : Visibility
{
    private Star star;


    //higher fog means that it is visible in the fog of war, lower fog is invisible.
    public int fogVisibility = 0;



    private void Awake()
    {

       star = GetComponent<Star>();

    }
    // Start is called before the first frame update
    void Start()
    {
       // starShipManager = GetComponent<StarShipManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Initialise()
    {
        star = GetComponent<Star>();

        base.Initialise();

        SetVisibility(false);
      
    }

    public override void SetVisibility(bool visible)
    {
        
        
        base.SetVisibility(visible);

        if (!fogOfWarEnabled)
        {
            visible = true;
        }
        
        for(int i = 0; i < star.starConnections.lines.Count; i++)
        {
            if (star.starConnections.lines[i] != null)
            {
                star.starConnections.lines[i].enabled = visible;
            }
            
        }
        if(star.starShipManager.iconHandler != null)
        {
            star.starShipManager.iconHandler.gameObject.SetActive(visible);
        }

        for(int i = 0; i < star.starShipManager.fleets.Count; i++)
        {
            star.starShipManager.fleets[i].UpdateVisibility();
        }

        
    }

    //0 is just this system, 1 is one away, 2 is 2 away ect.
    public void IncrementFogOfWar(int increment,int distance = 0, int sender = -1)
    {
        
        fogVisibility += increment;


        if ((fogVisibility > 0) != visibility)
        {
            SetVisibility(fogVisibility > 0);
        }

        if (distance <= 0)
        {
            return;
        }
        else
        {
            distance--;
        }
       
        for (int i = 0; i < star.starConnections.connections.Count; i++)
        {
            
            if (star.starConnections.connections[i] == sender )
            {
                continue;
            }
            
            Master.instance.enviroment.stars[star.starConnections.connections[i]].starVisibility.IncrementFogOfWar(increment,distance,star.index);
        }
    }

    public void SetFogOfWar(int amount, bool reset = false, int sender = -1)
    {
        if(fogVisibility >= amount && !reset)
        {
            return;
        }

        fogVisibility = amount;


        if ((fogVisibility > 0) != visibility)
        {
            SetVisibility(fogVisibility > 0);
        }
        
        if(fogVisibility <= 0)
        {
            return;
        }

        for (int i = 0; i < star.starConnections.connections.Count; i++)
        {
            if(star.starConnections.connections[i] == sender)
            {
                continue;
            }
            Master.instance.enviroment.stars[star.starConnections.connections[i]].starVisibility.SetFogOfWar(fogVisibility-1,reset,star.index);
        }
    }
}
