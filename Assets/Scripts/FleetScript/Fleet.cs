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

public interface IOrder
{
    Star TargetStar { get; }
    Transform TargetPoint { get; }
    List<Star> PreferedPath { get; }

    void OnReachStar(Fleet fleet, Star star);
    void OnReachPoint(Fleet fleet, Transform point);
}

[RequireComponent(typeof(FleetCombat))]
public class Fleet : MonoBehaviour
{
    public FleetCombat fleetCombat;

    [Header("GOAL MANAGEMENT")]
    readonly Queue<IOrder> orders = new Queue<IOrder>();
    IOrder currentOrder = null;
    FleetState fleetState = FleetState.IDLE;
    public ConflictReaction conflictReaction = ConflictReaction.FLEE;
    public bool isPath = false;

    int pathIndex = 0;
    List<Star> path = null;
    [Header("Faction")]
    public int faction = -1;

    [Header("SPACESHIP MOVEMENT")]
    public Transform target;
    public List<SpaceShip> spaceShips = new List<SpaceShip>();

    private const float targetTime = 0.25f;
    private const float proximity = 1.0f;
    private float targetTimer = 0;
    private float targetAngle = 0;

    [Header("SPACESHIP WARP")]
    [HideInInspector] public int iconHandlerID;

    public GameObject spaceShipHandler;
    public GameObject movingIcon;
    public Star star;
    private Star previousStar;
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

    private void Update()
    {
        if (fleetState != FleetState.CHARGING_WARP && fleetState != FleetState.IN_WARP)
        {
            UpdateGoals();
        }

        switch (fleetState)
        {
            case FleetState.MOVING_TO_POINT:
                if (MoveShipsTo(target))
                {
                   
                    if (currentOrder != null && currentOrder.TargetPoint == target)
                    {
                        currentOrder.OnReachPoint(this, target);
                        currentOrder = null;
                    }

                    target = null;
                    SetFleetState(FleetState.IDLE);
                }
                break;
            case FleetState.MOVING_TO_WARP_GATE:
                if (MoveShipsTo(target))
                {
                    if (currentOrder != null && currentOrder.TargetPoint == target)
                    {
                        currentOrder.OnReachPoint(this, target);
                        currentOrder = null;
                    }

                    target = null;
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
        spaceShip.SetFleet(this);
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

    public bool Busy()
    {
        return !(fleetState != FleetState.CHARGING_WARP && fleetState != FleetState.IN_WARP);
    }

    public void AddOrder(IOrder order)
    {
        orders.Enqueue(order);
    }


    public void ClearOrders()
    {
        if (!Busy())
        {
            orders.Clear();
            currentOrder = null;
            target = null;
            ClearPath();
            SetFleetState(FleetState.IDLE);
        }
    }

    public void ReactToConflict(bool conflict)
    {
        if (conflict)
        {
            switch (conflictReaction)
            {
                case ConflictReaction.FLEE:
                    AddOrder(null);//new TravelToPoint(this, CalculateFleeStar()), true);
                    break;
                case ConflictReaction.FIGHT:
                    SetFleetState(FleetState.FIGHTING);
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
            if (fleetState == FleetState.FIGHTING)
            {
                SetFleetState(FleetState.IDLE);
            }
            for (int i = 0; i < spaceShips.Count; i++)
            {
                spaceShips[i].SetConflict(conflict);
            }
        }
    }

    private Star CalculateFleeStar()
    {
        //go back to the previous star we came from:
        if (previousStar != null)
        {
            return previousStar;
        }

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
                return connectedStars[i];
            }
        }

        //all else fails, go to a random star.
        return connectedStars[Random.Range(0, connectedStars.Count)];
    }

    private void UpdateGoals()
    {
        if (currentOrder == null && orders.Count > 0)
        {
            currentOrder = orders.Dequeue();

            if(currentOrder.PreferedPath != null && currentOrder.PreferedPath[0] == star)
            {
                SetPath(currentOrder.PreferedPath);
            }
        }

        if (currentOrder != null)
        {
            if ((isPath && currentOrder.TargetStar != path[path.Count - 1]) || (!isPath && currentOrder.TargetStar != star))
            {
                SetPath(currentOrder.TargetStar);
            }

            if (target == null)
            {
                if (isPath)
                {
                    target = TargetWarpGate(path[pathIndex + 1]);
                    fleetState = FleetState.MOVING_TO_WARP_GATE;
                }
                else if (currentOrder.TargetStar == star)
                {
                    target = currentOrder.TargetPoint;
                    fleetState = FleetState.MOVING_TO_POINT;
                }
            }
        }
    }

    private bool MoveShipsTo(Transform point)
    {
        targetTimer -= Time.deltaTime;
        if (targetTimer <= 0.0f)
        {
            targetTimer = targetTime;

            //Calculate center of fleet
            Vector2 center = Vector2.zero;
            for (int i = 0; i < spaceShips.Count; ++i)
            {
                center += (Vector2)spaceShips[i].transform.position;
            }

            center /= spaceShips.Count;

            //Calculate angle to target
            Vector2 difference = (center - (Vector2)point.position);
            targetAngle = (Mathf.Rad2Deg * Mathf.Atan2(difference.y, difference.x));
            targetAngle += 90f;

            //Check if reach target
            if (difference.sqrMagnitude <= proximity)
            {
                return true;
            }
        }

        for (int i = 0; i < spaceShips.Count; ++i)
        {
            spaceShips[i].RotateTowards(targetAngle);
            spaceShips[i].Move();
        }

        return false;
    }

    private void SetPath(Star star)
    {
        path = Master.instance.PathFind(this.star, star);
        pathIndex = 0;
        isPath = true;
    }

    private void SetPath(List<Star> path)
    {
        this.path = path;
        pathIndex = 0;
        isPath = true;
    }

    private void ClearPath()
    {
        if (!isPath)
            return;

        if (fleetState == FleetState.MOVING_TO_WARP_GATE || fleetState == FleetState.CHARGING_WARP)
        {
            ClearTarget();
        }

        path.Clear();
        isPath = false;
    }

    private void ClearTarget(bool setToIdle = false)
    {
        this.target = null;

        if (setToIdle)
        {
            SetFleetState(FleetState.IDLE);
        }
    }

    //Tells the fleet to start trying to go to a certain star.
    public Transform TargetWarpGate(Star target)
    {
        Transform gate = star.starConnections.GetStarGate(target);
        warpStart = gate.position;
        warpEnd = target.starConnections.GetStarGate(star).position;
        SetFleetState(FleetState.MOVING_TO_WARP_GATE);
        this.target = gate;
        return gate;
    }

    //allows you to add code that happens when a certain state is set or removed.
    private void SetFleetState(FleetState fleetState)
    {
        switch (this.fleetState)
        {

            case FleetState.CHARGING_WARP:

                CancelInvoke(nameof(BeginWarp));
                break;

        }

        this.fleetState = fleetState;
        switch (this.fleetState)
        {

            case FleetState.CHARGING_WARP:
                ClearTarget();
                Invoke(nameof(BeginWarp), 3.0f);
                break;

        }
    }


    //Once fully charged, call this function to warp the fleet to target warp coordinates.
    private void BeginWarp()
    {
        if (!star.starShipManager.RequestExit(this))
        {
            return;
        }

        int line = star.starConnections.GetConnectionToStar(path[pathIndex + 1]);
        transform.SetParent(star.starConnections.lines[line].transform);

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
            previousStar = star;
            star = path[pathIndex + 1];
            LandShips(warpEnd);
            pathIndex++;

            if (currentOrder != null && currentOrder.TargetStar == star)
            {
                currentOrder.OnReachStar(this, star);
            }

            if (isPath)
            {
                if (pathIndex < path.Count - 1)
                {
                    TargetWarpGate(path[pathIndex + 1]);
                }
                else
                {
                    ClearPath();
                }
            }

            //this needs to be done last in-case there is a entry-reaction (such as a conflict) changing the fleets next step.
            star.starShipManager.Entry(this);
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
    }

    public void Scout(bool show)
    {
        star.starVisibility.IncrementFogOfWar(show ? 1 : -1, scoutingRange);
    }

    public void UpdateVisibility()
    {
        bool visible = star.starVisibility.visibility;
        for (int i = 0; i < spaceShips.Count; i++)
        {
            spaceShips[i].SetVisibility(visible);
        }

        movingIcon.SetActive(false);
    }
}

