using System.Collections.Generic;
using UnityEngine;

//the Navigator warp script handles a navigators current whereabouts, and moving from system to system.
public class NavigatorWarp : MonoBehaviour
{

    public GameObject spaceShipHandler;
    public GameObject movingIcon;

    private Navigator navigator;
    private bool isPath = false;
    private bool isWarping = false;
    private float warpAlpha = 0.0f;
    private Vector2 warpStart;
    private Vector2 warpEnd;

    private List<int> path = new List<int>();
    private int pathIndex = 0;

    public Star star;

    public bool IsWarping()
    {
        return isWarping;
    }

    public void SetIsPath(bool isPath)
    {
        this.isPath = isPath;
    }

    public bool IsPath()
    {
        return isPath;
    }

    public void Initialise(Navigator navigator)
    {
        this.navigator = navigator;
    }

    //sets the path for the navigator to follow. Returns the next star gate the fleet will first need to move to.
    //remember to set the fleet to formation after the destination is returned.(so that the fleet can move as one more efficiently).
    public Transform SetPath(List<int> pathSet)
    {
        Transform destination;
        pathIndex = 0;
        path = new List<int>(pathSet);
        isPath = true;

        destination = star.starConnections.GetStarGate(pathSet[1]);
        if (destination == null)
        {
            Debug.LogError("gate is null");
            return null;
        }

        return destination;
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
    }


    //remember to set the fleet to NOT formation before begining warp. 
    public bool BeginWarp(int destinationCoordinates)
    {

        int line = star.starConnections.GetConnectionToStar(destinationCoordinates);
        transform.SetParent(star.starConnections.lines[line].transform);


        //begin warp through the gate.
        movingIcon.SetActive(star.starVisibility.visibility);
        spaceShipHandler.SetActive(false);

        isWarping = true;
        warpAlpha = 0.0f;

        warpStart = Master.instance.enviroment.stars[star.index].starConnections.GetStarGate(destinationCoordinates).position;
        warpEnd = Master.instance.enviroment.stars[destinationCoordinates].starConnections.GetStarGate(star.index).position;

        return isWarping;
    }
    public bool BeginWarp()
    {
        return BeginWarp(path[pathIndex + 1]);
    }


    //could play a cool animation or something.Returns true when it's done.
    public bool WarpUpdate()
    {
        warpAlpha += Time.deltaTime / 3f;
        transform.position = Vector2.Lerp(warpStart, warpEnd, warpAlpha);

        if (warpAlpha >= 1f)
        {
            pathIndex++;
            WarpEnd(path[pathIndex], warpEnd);

            //return to idle if at the end of the path.
            if (pathIndex >= path.Count - 1)
            {
                navigator.MoveToDestination(star.transform);
                navigator.OnFinishPath();
                return true;
            }
            //go to next star gate
            else
            {
                navigator.MoveToNextStarGate(path[pathIndex + 1]);
            }

        }
        return false;

    }

    public Star WarpEnd(int newStarCoordinate, Vector2 endPosition)
    {
        if (!isWarping)
        {
            //OnSuddenWarp();
            star.starShipManager.RequestExit(navigator);
        }
        isWarping = false;
        star = Master.instance.enviroment.stars[newStarCoordinate];
        star.starShipManager.Entry(navigator);
        LandShips(endPosition);

        return star;
    }

    //only needs the navigator for updating the ships positions.
    private void LandShips(Vector2 endPosition)
    {
        movingIcon.SetActive(false);
        spaceShipHandler.SetActive(true);
        navigator.SetShipsPosition(endPosition);
    }
}
