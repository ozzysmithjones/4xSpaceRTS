using System.Collections.Generic;
using UnityEngine;

public class MoveFleetTool : Tool
{
    public LineRenderer lineRenderer;
    public List<Fleet> controlledFleets = new List<Fleet>();
    private List<int> path = new List<int>();

    public int start = -1;

    private bool ignoreShutDown = false;
    //returns true if this was an override, otherwise returns false.
    public bool AddFleet(Fleet fleet)
    {
        if (Master.instance.userInterface.currentTool != this)
        {
            Master.instance.userInterface.SetTool(3);
        }


        fleet.ClearPath();
        if (fleet.star.index != start)
        {
            ignoreShutDown = true;

            UnToggleFleetIcons();
            controlledFleets.Clear();
            controlledFleets.Add(fleet);

            ignoreShutDown = false;

            start = controlledFleets[0].star.index;
            path.Clear();
            path.Add(start);

            return true;
        }
        else
        {
            controlledFleets.Add(fleet);
            return false;
        }
    }

    //returns true if it was the last fleet left in the move tool, otherwise returns false.
    public bool RemoveFleet(Fleet fleet)
    {
        fleet.star.starShipManager.iconHandler.FindIcon(fleet.iconHandlerID).setToggleOn(false);
        controlledFleets.Remove(fleet);
        if (controlledFleets.Count <= 0 && !ignoreShutDown)
        {
            path.Clear();
            start = -1;
            Master.instance.userInterface.SetTool(0);
            return true;
        }

        return false;
    }


    public override void OnSelected()
    {
        base.OnSelected();



    }

    void DrawLine(List<int> linePath)
    {
        lineRenderer.positionCount = linePath.Count;
        for (int i = 0; i < linePath.Count; i++)
        {
            lineRenderer.SetPosition(i, Master.instance.enviroment.stars[linePath[i]].transform.position + new Vector3(0f, 0f, 3f));
        }
    }

    public override void OnInteract(Star star)
    {
        base.OnInteract(star);

        if (path[path.Count - 1] == star.index)
        {
            return;
        }

        bool addition = path.Count > 0;

        List<int> extension = Master.instance.PathFind(path[path.Count - 1], star.index);
        if (addition)
        {
            extension.RemoveAt(0);
        }
        path.AddRange(extension);
        DrawLine(path);


        if (Input.GetKey(KeyCode.Mouse1))
        {
            //Debug.Log("right click, so ignore");
            return;
        }
        else
        {
            //Debug.Log("left clcik, so end");
            Master.instance.userInterface.SetTool(0);
        }



    }


    public override void OnHover(Star star)
    {
        base.OnHover(star);

        //just draw a line:
        int startIndex = path.Count - 1;
        List<int> extension = Master.instance.PathFind(path[startIndex], star.index);
        List<int> total = new List<int>(path);

        total.AddRange(extension);
        DrawLine(total);
    }

    public override void OnDeselected()
    {
        //Debug.Log("deselected");
        base.OnDeselected();

        lineRenderer.positionCount = 0;

        if (path.Count > 1)
        {
            for (int i = 0; i < controlledFleets.Count; i++)
            {
                controlledFleets[i].ClearPath();
                controlledFleets[i].ClearFleetOrder();
                controlledFleets[i].AddFleetOrder(new TravelToPoint(controlledFleets[i],path[path.Count-1],null,path));

            }
        }

        UnToggleFleetIcons();
    }

    void UnToggleFleetIcons(int exception = -1)
    {
        for (int i = 0; i < controlledFleets.Count; i++)
        {
            if (i == exception)
            {
                continue;
            }
            controlledFleets[i].star.starShipManager.iconHandler.FindIcon(controlledFleets[i].iconHandlerID).setToggleOn(false);
        }
    }








}
