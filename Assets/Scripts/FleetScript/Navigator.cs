using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum NavigatorState
{
    IDLE,
    TRAVELING,
    FIGHTING

}
[RequireComponent(typeof(NavigatorWarp))]
[RequireComponent(typeof(NavigatorCombat))]
public class Navigator : MonoBehaviour
{

    public Transform shipsParent;
    private bool initialised = false;


    //The navigator is the parent class to all fleets and civilian ships in the game. 
    //A navigator can be moved from system to system, with an icon on the zoomed out screen displaying its current whereabouts. 
    public Transform center;

    public int iconHandlerID = -1;
    public Sprite iconSprite;

    public List<SpaceShip> spaceShips;
    private float targetAngle = 0.0f;
    
    public int scoutingRange = 1;

    public bool military = true;
    public int faction = 0;
    public NavigatorCombat navigatorCombat;
    protected NavigatorWarp navigatorWarp;

    private Timer starGateProximityTimer;
    public Transform destination;

    private bool moveToDestination = false;
    private Timer destinationProximityTimer;
    private Timer targetAngleTimer;



    private void Awake()
    {
        Initialise();
    }
    public virtual void Initialise()
    {
        if (initialised)
        {
            return;
        }
        targetAngleTimer = new Timer(0.25f, UpdateTargetAngle);
        destinationProximityTimer = new Timer(0.5f, IdleScan);
        starGateProximityTimer = new Timer(1.0f, StarGateScan);

        navigatorCombat = GetComponent<NavigatorCombat>();
        navigatorWarp = GetComponent<NavigatorWarp>();
        navigatorWarp.Initialise(this);

        initialised = true;
    }

    public void SetStar(Star star)
    {
        navigatorWarp.star = star;
    }

    public Star GetStar()
    {
        return navigatorWarp.star;
    }
    public virtual void ConflictReaction(bool conflict)
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].SetConflict(conflict);
        }
    }

    public void AddShip(SpaceShip spaceShip)
    {
        spaceShip.SetNavigator(this);
        spaceShips.Add(spaceShip);
        spaceShip.transform.SetParent(shipsParent);
    }

    

    public void RemoveShip(SpaceShip spaceShip)
    {
        spaceShips.Remove(spaceShip);

        //destroy fleet, if there are no ships left.
        if (spaceShips.Count <= 0)
        {
            GetStar().starShipManager.RequestExit(this);
            RemoveFromFaction(faction);
            Destroy(gameObject);
        }

    }
    private void OnDestroy()
    {
       //remove from the fleet tool.
       if (faction == 0)
       {
          if (Master.instance.userInterface.moveFleetTool.controlledFleets.Contains(this))
          {
             Master.instance.userInterface.moveFleetTool.RemoveFleet(this);
          }
       }
        //TODO: remove from the AI.(if the AI is currently holding a reference to it)

        if (!navigatorWarp.IsWarping())
        {
            GetStar().starShipManager.RequestExit(this);
        }
         
    }

    public void Scout(bool show)
    {

        GetStar().starVisibility.IncrementFogOfWar(show ? 1 : -1, scoutingRange);
    }

    public void SetPath(List<int> pathSet)
    {

        destination = navigatorWarp.SetPath(pathSet);

        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = true;
        }
    }

    public void ClearPath()
    {

        navigatorWarp.ClearPath();
       
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].ClearPath();
        }
    }

    public void MoveToNextStarGate(int nextStarCoordinate)
    {
        destination = GetStar().starConnections.GetStarGate(nextStarCoordinate);

        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = true;
        }
    }


    public void MoveToDestination(Transform newDestination)
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = true;
        }
        navigatorWarp.SetIsPath(false);
        moveToDestination = true;
        destination = newDestination;
    }

    private void Update()
    {
        
        if (navigatorWarp.IsPath() && navigatorWarp.IsWarping())
        {
            navigatorWarp.WarpUpdate();
        }
        else
        {
          center.position = (Vector3)AveragePosition();  
          if (navigatorWarp.IsPath())
          {
                if (UpdateFormation())
                {
                    starGateProximityTimer.Tick(Time.deltaTime);
                }
          }
          else if (moveToDestination)
          {
                if (UpdateFormation())
                {
                    destinationProximityTimer.Tick(Time.deltaTime);
                }

           }
            
        }
    }

    void StarGateScan(){
        float distance = Vector2.Distance((Vector2)center.position, (Vector2)destination.position);

        if(distance <= 3f)
        {
            if (!GetStar().starShipManager.RequestExit(this))
            {
                return;
            }

            navigatorWarp.BeginWarp();

            for (int i = 0; i < spaceShips.Count; i++)
            {
                spaceShips[i].formation = false;
            }

        }

    }

    void IdleScan()
    {
        float distance = Vector2.Distance((Vector2)center.position, (Vector2)destination.position);

        if(distance <= 1f)
        {
            moveToDestination = false;
            for (int i = 0; i < spaceShips.Count; i++)
            {
                spaceShips[i].formation = false;
            }
            OnFinishIdle();
        }
    }

    Vector2 AveragePosition()
    {
        Vector2 total = Vector2.zero;
        if (spaceShips.Count > 0)
        {
            for (int i = 0; i < spaceShips.Count; i++)
            {
                total.x += spaceShips[i].transform.position.x;
                total.y += spaceShips[i].transform.position.y;
            }
            //print(new Vector2(total.x / (float)spaceShips.Count, total.y / (float)spaceShips.Count));
            return new Vector2(total.x / (float)spaceShips.Count, total.y / (float)spaceShips.Count);
        }
        else
        {
            Debug.LogError("not getting center");
            return (Vector2)transform.position;
        }
    }

    public void SetShipsPosition(Vector2 position, float maxDeviancy = 3f){

        for(int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].transform.position = (Vector3)(position + Random.insideUnitCircle * maxDeviancy);
        }

        AveragePosition();
    }

   public void UpdateVisibility()
   {
        if(GetStar() == null)
        {
            return;
        }

        bool visible = GetStar().starVisibility.visibility;

        for(int i = 0; i < spaceShips.Count; i++)
        {

            spaceShips[i].SetVisibility(visible);
        }

        navigatorWarp.movingIcon.SetActive(visible);
        
   }

    public virtual void OnEnterStar()
    {

    }

    public virtual void OnLeaveStar()
    {

    }

    public virtual void OnFinishPath()
    {

    }

    public virtual void OnFinishIdle()
    {

    }
    public virtual void SetFaction(int factionIndex)
    {
        this.faction = factionIndex;
    }


    void UpdateTargetAngle()
    {

        //work out the target angle(I use trigonometry).
        Vector2 difference = (center.position - destination.position).normalized;

        float angle = (Mathf.Rad2Deg * Mathf.Atan2(difference.y, difference.x));

        angle += 90f;

        targetAngle = angle;
    }
    private bool UpdateFormation()
    {
        bool allInFormation = true;
        targetAngleTimer.Tick(Time.deltaTime);
        for (int i = 0; i < spaceShips.Count; i++)
        {
            if (spaceShips[i].spaceShipState == SpaceShipState.FORMATION)
            {
                spaceShips[i].RotateTowards(targetAngle);
                spaceShips[i].Move();
            }
            else
            {
                allInFormation = false;
            }

        }
        return allInFormation;

    }

    public void AddToFaction(int newFaction)
    {
        if(faction >= 0)
        {
            RemoveFromFaction(faction);
        }
        faction = newFaction;
        Master.instance.factions.factions[faction].fleets.Add(this);
    }

    public void RemoveFromFaction(int oldFaction)
    {
       
        Master.instance.factions.factions[faction].fleets.Remove(this);
        faction = -1;
    }
}
