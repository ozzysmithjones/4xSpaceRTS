using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum NavigatorState
{
    IDLE,
    TRAVELING,
    FIGHTING

}


[RequireComponent(typeof(NavigatorScouting))]
public class Navigator : MonoBehaviour
{

    public Transform shipsParent;
    private bool initialised = false;


    //The navigator is the parent class to all fleets and civilian ships in the game. 
    //A navigator can be moved from system to system, with an icon on the zoomed out screen displaying its current whereabouts. 
    public GameObject spaceShipHandler;
    public GameObject movingIcon;
    public Transform center;

    public int iconHandlerID = -1;
    public Sprite iconSprite;


    public List<SpaceShip> spaceShips;
    private float targetAngle = 0.0f;
    private List<int> path;
    private bool isPath = false;
    private int pathIndex = 0;



    private bool warping = false;
    private float warpAlpha = 0.0f;
    private Vector2 warpStart;
    private Vector2 warpEnd;

    public int scoutingRange = 1;

    public bool military = true;
    public int faction = 0;
    public NavigatorCombat navigatorCombat;



    public Star star;

    private Timer starGateProximityTimer;
    public Transform destination;

    private bool moveToIdle = false;
    private Timer idleProximityTimer;
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
        idleProximityTimer = new Timer(0.5f, IdleScan);
        starGateProximityTimer = new Timer(1.0f, StarGateScan);
        navigatorCombat = GetComponent<NavigatorCombat>();
        initialised = true;
    }

    public void AddShip(SpaceShip spaceShip)
    {
        spaceShip.SetNavigator(this);
        spaceShips.Add(spaceShip);
        spaceShip.transform.SetParent(shipsParent);
    }

    public virtual void ConflictReaction(bool conflict)
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].SetConflict(conflict);
        }
    }

    public void RemoveShip(SpaceShip spaceShip)
    {
        spaceShips.Remove(spaceShip);

        //destroy fleet, if there are no ships left.
        if (spaceShips.Count <= 0)
        {
            star.starShipManager.RequestExit(faction, this);
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

        if (!warping)
        {
            star.starShipManager.RequestExit(faction, this);
        }
         
    }

    public void Scout(bool show)
    {

        star.starVisibility.IncrementFogOfWar(show ? 1 : -1, scoutingRange);
    }

    public void SetPath(List<int> pathSet)
    {
        pathIndex = 0;
        path = new List<int>(pathSet);
        isPath = true;

        if(star == null)
        {
            Debug.LogError("star is null");
        }

        destination = star.starConnections.GetStarGate(pathSet[1]);
        if(destination == null)
        {
            Debug.LogError("gate is null");
        }

        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = true;
        }
    }

    

    public void ClearPath()
    {

        if (!isPath)
        {
            return;
        }
        pathIndex = 0;
        path.Clear();
        isPath = false;

        //send a signal to all the ships bound to this navigator to travel towards the nearest gateway.
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].ClearPath();
        }
    }

    private void MoveToNextStarGate(int nextStarCoordinate)
    {
        destination = star.starConnections.GetStarGate(nextStarCoordinate);

        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = true;
        }
    }

    private void MoveToIdle()
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = true;
        }
        isPath = false;
        moveToIdle = true;
        destination = star.transform;
    }
    private void LandShips(Vector2 endPosition)
    {
        movingIcon.SetActive(false);
        spaceShipHandler.SetActive(true);
        SetShipsPosition(endPosition);
    }

    public void WarpTo(int starCoordinate, Vector2 endPosition)
    {
        if (!warping)
        {
            OnSuddenWarp();
            star.starShipManager.RequestExit(faction, this);
        }
        warping = false;
        star = Master.instance.enviroment.grid[starCoordinate];
        star.starShipManager.Entry(faction, this);
        LandShips(endPosition);
    }

    protected virtual void OnSuddenWarp()
    {
        for(int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].OnSuddenWarp();
        }
    }

    public void WarpTo(int starCoordinate)
    {
        WarpTo(starCoordinate, Master.instance.enviroment.grid[starCoordinate].transform.position);
    }

    private void Update()
    {
        
        if (warping && isPath)
        {
            warpAlpha += Time.deltaTime / 3f;
            transform.position = Vector2.Lerp(warpStart, warpEnd, warpAlpha);

            if(warpAlpha >= 1f)
            {
                pathIndex++;
                WarpTo(path[pathIndex],warpEnd);
                //return to idle if at the end of the path.
                if (pathIndex >= path.Count - 1)
                {
                    MoveToIdle();
                    OnFinishPath();
                }
                //go to next star gate
                else
                {
                    MoveToNextStarGate(path[pathIndex + 1]);
                }
                
            }
        }
        else
        {
          center.position = (Vector3)AveragePosition();  
          if (isPath)
          {
                if (UpdateFormation())
                {
                    starGateProximityTimer.Tick(Time.deltaTime);
                }
          }
          else if (moveToIdle)
          {
                if (UpdateFormation())
                {
                    idleProximityTimer.Tick(Time.deltaTime);
                }

           }
            
        }
    }

    void StarGateScan(){
        float distance = Vector2.Distance((Vector2)center.position, (Vector2)destination.position);

        if(distance <= 3f)
        {
            if (!star.starShipManager.RequestExit(faction,this))
            {
                return;
            }

            //parent self to the connection

            if(pathIndex+1 >= path.Count || pathIndex+1 < 0)
            {
                Debug.LogError("path index is out of range, index =  "+ (pathIndex+1).ToString() + " array size = " + path.Count);
            }

            int line = star.starConnections.GetConnectionToStar(path[pathIndex + 1]);
            transform.SetParent(star.starConnections.lines[line].transform);

            for (int i = 0; i < spaceShips.Count; i++)
            {
                spaceShips[i].formation = false;
            }

            //begin warp through the gate.
            movingIcon.SetActive(star.starVisibility.visibility);
            spaceShipHandler.SetActive(false);

            warping = true;
            warpAlpha = 0.0f;

            warpStart = destination.position;
            warpEnd = Master.instance.enviroment.grid[path[pathIndex + 1]].starConnections.GetStarGate(star.position).position;
        }

    }

    void IdleScan()
    {
        float distance = Vector2.Distance((Vector2)center.position, (Vector2)destination.position);

        if(distance <= 1f)
        {
            moveToIdle = false;
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

    void SetShipsPosition(Vector2 position, float maxDeviancy = 3f){

        for(int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].transform.position = (Vector3)(position + Random.insideUnitCircle * maxDeviancy);
        }

        AveragePosition();
    }

   public void UpdateVisibility()
   {
        if(star == null)
        {
            return;
        }

        bool visible = star.starVisibility.visibility;

        for(int i = 0; i < spaceShips.Count; i++)
        {

            spaceShips[i].SetVisibility(visible);
        }

        movingIcon.SetActive(visible);
        
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








}
