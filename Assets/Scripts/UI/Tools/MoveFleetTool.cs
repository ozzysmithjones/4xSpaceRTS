using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class MoveFleetTool : Tool
{
    public LineRenderer lineRenderer;
    public List<Fleet> controlledFleets = new List<Fleet>();
    public List<int> path = new List<int>();

    public int start = -1;

    public bool AddFleet(Fleet fleet)
    {
        if (Master.instance.userInterface.currentTool != this)
        {
            Master.instance.userInterface.SetTool(3);
        }

        fleet.ClearPath();
        if (fleet.star.index != start)
        {
            UnToggleFleetIcons();
            controlledFleets.Clear();
            controlledFleets.Add(fleet);


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
    public bool RemoveFleet(Fleet fleet, bool clearPathIfLast = true)
    {
        fleet.star.starShipManager.iconHandler.FindIcon(fleet.iconHandlerID).setToggleOn(false);
        controlledFleets.Remove(fleet);
        if (controlledFleets.Count <= 0 && clearPathIfLast)
        {
            path.Clear();
            path.Add(start);
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            return;
        }
        else
        {
            Master.instance.userInterface.SetTool(0);
        }

    }

    public override void OnHover(Star star)
    {
        base.OnHover(star);

        if (controlledFleets.Count <= 0)
            return;
        if(star.index == start)
            return;

        for(int i = path.Count-1; i >= 0; i--)
        {
            if(path[i] == star.index)
            {
                path.RemoveRange(i, path.Count - i);
                DrawLine(path);
                return;
            }
        }

        List<int> extension = Master.instance.PathFind(path[path.Count - 1], star.index);
        if (extension.Count <= 1)
        {
            return;
        }
        extension.RemoveAt(0);
        path.AddRange(extension);
        DrawLine(path);

    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        start = -1;
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
        controlledFleets.Clear();
    }

    void UnToggleFleetIcons()
    {
        for (int i = 0; i < controlledFleets.Count; i++)
        {
            controlledFleets[i].star.starShipManager.iconHandler.FindIcon(controlledFleets[i].iconHandlerID).setToggleOn(false);
        }
    }








}
