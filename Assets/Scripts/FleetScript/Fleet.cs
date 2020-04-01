using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script handles fleet movement and following orders.

public enum FleetState
{
    IDLE,
    FIGHTING,
    MOVING_TO_POINT,
    MOVING_TO_WARP_GATE,
    CHARGING_WARP,
    IN_WARP
}

public enum ConflictReaction
{
    FLEE,
    IGNORE,
    FIGHT,
}

[RequireComponent(typeof(FleetCombat))]
public class Fleet : MonoBehaviour
{
    public FleetCombat fleetCombat;
    [Header("GOAL MANAGEMENT")]
    public bool isPath = false;
    public ConflictReaction conflictReaction = ConflictReaction.FLEE;
    public FleetState fleetState = FleetState.IDLE;

    public List<FleetOrder> fleetOrders = new List<FleetOrder>();
    private int currentFleetOrderIndex = 0;

    [SerializeField] private List<int> path;
    private int pathIndex = 0;

    [Header("FACTION AFFILIATION")]
    public int faction = -1;

    [Header("SPACESHIP MOVEMENT")]
    public Transform center;
    private float targetAngle;
    public List<SpaceShip> spaceShips = new List<SpaceShip>();
    private bool inFormation = false;

    private float proximityDistance = 3.0f;
    [SerializeField] private bool isTarget = false;
    [SerializeField] private bool isCloseToTarget = false;
    [SerializeField] private Transform target;

    [Header("SPACESHIP WARP")]
    [HideInInspector] public int iconHandlerID;
    [SerializeField] private bool usingGate = false;
    private int targetWarpCoordinate;

    [SerializeField] private GameObject spaceShipHandler;
    [SerializeField] private GameObject movingIcon;
    public Star star;
    private float warpAlpha = 0.0f;
    private Vector2 warpStart;
    private Vector2 warpEnd;

    [Header("CUSTOMISABLE")]
    public bool military = true;
    public Sprite iconSprite;
    public int scoutingRange = 1;

    private void Awake()
    {
        fleetCombat = GetComponent<FleetCombat>();
    }
    private void Start()
    {
        InvokeRepeating("DistanceCheck", 0.5f, 0.5f);
        InvokeRepeating("UpdateTargetAngle", 0.25f, 0.25f);
    }

    private void Update()
    {
        if(fleetState != FleetState.IN_WARP && fleetState != FleetState.CHARGING_WARP)
            UpdateFleetOrder();
        switch (fleetState)
        {
            case FleetState.MOVING_TO_POINT:        
                UpdateFormation();
                if (isCloseToTarget)
                {
                    ClearTarget();
                }
                break;
            case FleetState.MOVING_TO_WARP_GATE:
                UpdateFormation();
                if (isCloseToTarget)
                {
                    //setting the fleet state to charging will automatically call the warp behaviour.
                    ClearTarget(false);
                    SetFleetState(FleetState.CHARGING_WARP);
                }
                break;
            case FleetState.IN_WARP:
                UpdateWarp();
                break;
            case FleetState.FIGHTING:

                break;
            default:
                //just do nothing, or play an animation who knows.
                break;
        }
    }

    private void UpdateFleetOrder()
    {
        if (currentFleetOrderIndex >= fleetOrders.Count)
        {
            ClearFleetOrder();
            return;
        }
        if (currentFleetOrderIndex < fleetOrders.Count)
        {
            if (!fleetOrders[currentFleetOrderIndex].initialised)
            {
                fleetOrders[currentFleetOrderIndex].Initialise(this);
            }

            if (fleetOrders[currentFleetOrderIndex].Execute())
            {
                currentFleetOrderIndex++;
            }
        }
    }

    private void OnDestroy()
    {
        if (faction == 0)
        {
            if (Master.instance.userInterface.moveFleetTool.controlledFleets.Contains(this))
            {
                Master.instance.userInterface.moveFleetTool.RemoveFleet(this);
            }
        }

        if (fleetState != FleetState.IN_WARP)
        {
            star.starShipManager.RequestExit(this);
        }
    }

    public void ReactToConflict(bool conflict)
    {
        if (conflict)
        {
            switch (conflictReaction)
            {
                case ConflictReaction.FLEE:
                    ReadyWarpTo(FleeCoordinate(), true);
                    break;
                case ConflictReaction.FIGHT:
                    for (int i = 0; i < spaceShips.Count; i++)
                    {
                        spaceShips[i].SetConflict(conflict);
                    }
                    break;
                case ConflictReaction.IGNORE:
                    //do nothing.
                    break;
            }
        }

        if (!conflict)
        {
            for (int i = 0; i < spaceShips.Count; i++)
            {
                spaceShips[i].SetConflict(conflict);
            }
        }
    }

    private int FleeCoordinate()
    {
        //go back the path we came.
        if (isPath && pathIndex > 0)
        {
            return path[pathIndex - 1];
        }
        //first star that's within our empire.
        List<Star> connectedStars = star.starConnections.GetConnectedStars();
        for (int i = 0; i < connectedStars.Count; i++)
        {
            if (connectedStars[i].factionIndex == faction)
            {
                return connectedStars[i].index;
            }
        }

        //all else fails, go to a random star.
        return connectedStars[Random.Range(0, connectedStars.Count)].index;
    }

    public void AddToFaction(int newFaction)
    {
        if (faction >= 0)
        {
            RemoveFromFaction(faction);
        }
        faction = newFaction;
        Master.instance.characters.factions[faction].fleets.Add(this);
    }
    public void RemoveFromFaction(int oldFaction)
    {

        Master.instance.characters.factions[faction].fleets.Remove(this);
        faction = -1;
    }

    public void AddShip(SpaceShip spaceShip)
    {
        spaceShip.formation = inFormation;
        spaceShips.Add(spaceShip);

        spaceShip.transform.SetParent(spaceShipHandler.transform);
    }
    public void RemoveShip(SpaceShip spaceShip)
    {
        spaceShips.Remove(spaceShip);

        if (spaceShips.Count <= 0)
        {
            star.starShipManager.RequestExit(this);
            Destroy(gameObject);
        }
    }

    public void AddFleetOrder(FleetOrder fleetOrder, bool doFirst = false)
    {
        if (!doFirst)
        {
            fleetOrders.Add(fleetOrder);
        }
        else
        {
            fleetOrders.Insert(currentFleetOrderIndex, fleetOrder);
        }
    }

    public void ClearFleetOrder()
    {
        fleetOrders.Clear();
        currentFleetOrderIndex = 0;
    }

    //allows you to add code that happens when a certain state is set or removed.
    private void SetFleetState(FleetState fleetState)
    {
        switch (this.fleetState)
        {

            case FleetState.CHARGING_WARP:
                
                CancelInvoke("BeginWarp");
                break;

        }
        this.fleetState = fleetState;
        switch (this.fleetState)
        {

            case FleetState.CHARGING_WARP:
                ClearTarget();
                Invoke("BeginWarp", 3.0f);
                break;

        }
    }
    //do not directly call set path unless part of a fleet order. Make fleets move by adding fleet orders.
    public void SetPath(List<int> path, bool usingGates = true)
    {
        isPath = true;
        this.pathIndex = 0;
        this.usingGate = usingGates;
        this.path = new List<int>();

        for(int i = 0; i < path.Count; i++)
        {
            this.path.Add(path[i]);
        }

        ReadyWarpTo(path[1], usingGate);

    }
    public void ClearPath()
    {
        isPath = false;
        this.path.Clear();
    }
    //Targets can only be within the star the fleet is in.
    public void SetTarget(Transform target, bool isWarpGate = false)
    {
        this.target = target;
        isTarget = true;
        SetFormation(true);
        SetFleetState(isWarpGate ? FleetState.MOVING_TO_WARP_GATE : FleetState.MOVING_TO_POINT);

    }
    public void ClearTarget(bool setToIdle = true)
    {
        this.target = null;
        isTarget = false;
        isCloseToTarget = false;
        SetFormation(false);
        if (setToIdle)
        {
            SetFleetState(FleetState.IDLE);
        }
    }

    private void DistanceCheck()
    {
        if (!isTarget)
        {
            isCloseToTarget = false;
            return;
        }
        if(this.fleetState != FleetState.MOVING_TO_POINT && this.fleetState != FleetState.MOVING_TO_WARP_GATE)
        {
            isCloseToTarget = false;
            return;
        }
        UpdateCenter();
        float distance = Vector2.Distance(center.position,target.position);
        isCloseToTarget = (distance < proximityDistance);
    }
    private void UpdateTargetAngle()
    {
        if (!isTarget)
        {
            return;
        }
        UpdateCenter();
        Vector2 difference = (center.position - target.position).normalized;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(difference.y, difference.x));
        angle += 90f;

        targetAngle = angle;
    }
    public void UpdateVisibility()
    {
        bool visible = star.starVisibility.visibility;
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].SetVisibility(visible);
        }
        movingIcon.SetActive(visible);
    }

    private void UpdateCenter()
    {
        //Get average position.
        Vector2 total = new Vector2(0.0f, 0.0f);
        for (int i = 0; i < spaceShips.Count; i++)
        {
            total += (Vector2)spaceShips[i].transform.position;
        }

        center.position = new Vector2(total.x / spaceShips.Count, total.y / spaceShips.Count);
    }

    private void UpdateFormation()
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            if (spaceShips[i].spaceShipState == SpaceShipState.FORMATION)
            {
                spaceShips[i].RotateTowards(targetAngle);
                spaceShips[i].Move();
            }

        }
    }

    private void SetFormation(bool formation)
    {
        inFormation = formation;
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].formation = formation;
        }
    }

    //Tells the fleet to start trying to go to a certain star.If using a gate, then the fleet will move to one and use it.
    //(otherwise it will warp from it's position).
    public void ReadyWarpTo(int coordinate, bool usingGate = true)
    {
        targetWarpCoordinate = coordinate;

        if (usingGate)
        {
            Transform gate = Master.instance.enviroment.stars[star.index].starConnections.GetStarGate(coordinate);
            warpStart = gate.position;
            warpEnd = Master.instance.enviroment.stars[coordinate].starConnections.GetStarGate(star.index).position;

            SetTarget(gate, true);
        }
        else
        {
            warpStart = transform.position;
            warpEnd = Master.instance.enviroment.stars[coordinate].transform.position;
            SetFleetState(FleetState.CHARGING_WARP);
        }

    }
    //Once fully charged, call this function to warp the fleet to target warp coordinates.
    private void BeginWarp()
    {

        if (!star.starShipManager.RequestExit(this))
        {
            return;
        }
        if (usingGate)
        {
            int line = star.starConnections.GetConnectionToStar(targetWarpCoordinate);
            transform.SetParent(star.starConnections.lines[line].transform);
        }
        else
        {
            transform.SetParent(star.transform.parent);
        }

        movingIcon.SetActive(star.starVisibility.visibility);
        spaceShipHandler.SetActive(false);
        warpAlpha = 0.0f;
        SetFleetState(FleetState.IN_WARP);
    }

    private void UpdateWarp(float timeRequired = 3.0f)
    {
        warpAlpha += Time.deltaTime / timeRequired;
        transform.position = Vector2.Lerp(warpStart, warpEnd, warpAlpha);

        if (warpAlpha >= 1f)
        {
            SetFleetState(FleetState.IDLE);
            warpAlpha = 0.0f;
            star = Master.instance.enviroment.stars[targetWarpCoordinate];
            star.starShipManager.Entry(this);
            LandShips(warpEnd);

            if (isPath)
            {
                pathIndex++;
                if (star.index == path[pathIndex])
                {
                    if (pathIndex < path.Count-1)
                        ReadyWarpTo(path[pathIndex + 1], usingGate);
                    else
                        ClearPath();
                }
                else
                {
                    AddFleetOrder(new TravelToStar(path[pathIndex], usingGate), true);
                }
            }
        }

    }

    private void LandShips(Vector2 endPosition, float maxDeviancy = 3.0f)
    {
        movingIcon.SetActive(false);
        spaceShipHandler.SetActive(true);

        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].transform.position = (Vector3)(endPosition + Random.insideUnitCircle * maxDeviancy);
        }

        UpdateCenter();
    }

    public void Scout(bool show)
    {
        star.starVisibility.IncrementFogOfWar(show ? 1 : -1, scoutingRange);
    }


}

