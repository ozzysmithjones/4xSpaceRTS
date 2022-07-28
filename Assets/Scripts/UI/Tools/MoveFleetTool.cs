using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class MoveFleetTool : Tool
{
    public LineRenderer lineRenderer;
    public List<Fleet> controlledFleets = new List<Fleet>();
    public List<Star> path = new List<Star>();

    public Star start = null;

    private Transform targetPoint;
    private Planet targetPlanet;


    public void AddFleet(Fleet fleet)
    {
        if(fleet.Busy())
        {
            return;
        }

        if (Master.instance.userInterface.currentTool != this)
        {
            Master.instance.userInterface.SetTool(3);
        }

        fleet.ClearOrders();
        if (fleet.star != start)
        {
            UnToggleFleetIcons();
            controlledFleets.Clear();
            controlledFleets.Add(fleet);


            start = fleet.star;
            path.Clear();
            path.Add(start);
        }
        else
        {
            controlledFleets.Add(fleet);
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

    void DrawLine(List<Star> linePath)
    {
        lineRenderer.positionCount = linePath.Count + (targetPoint != null ? 1 : 0);
        for (int i = 0; i < linePath.Count; i++)
        {
            lineRenderer.SetPosition(i, linePath[i].transform.position + new Vector3(0f, 0f, 3f));
        }

        if(targetPoint != null)
        {
            lineRenderer.SetPosition(linePath.Count, targetPoint.position + new Vector3(0f, 0f, 3f));
        }
    }

    public override void OnInteractStar(Star star)
    {
        base.OnInteractStar(star);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            return;
        }
        else
        {
            Master.instance.userInterface.SetTool(0);
        }

    }

    public override void OnHoverStar(Star star)
    {
        base.OnHoverStar(star);

        if (controlledFleets.Count <= 0)
            return;
        if(star == start)
            return;

        for(int i = path.Count-1; i >= 0; i--)
        {
            if(path[i] == star)
            {
                path.RemoveRange(i, path.Count - i);
                DrawLine(path);
                return;
            }
        }

        if (path[path.Count - 1] != star)
        {
            List<Star> extension =  Master.instance.PathFind(path[path.Count - 1], star);
            if (extension.Count <= 1)
            {
                return;
            }
            extension.RemoveAt(0);
            path.AddRange(extension);
        }
        

        DrawLine(path);
    }

    public override void OnInteractPlanet(Planet planet)
    {
        base.OnInteractPlanet(planet);
        Master.instance.userInterface.SetTool(0);

    }

    public override void OnHoverPlanet(Planet planet)
    {
        base.OnHoverPlanet(planet);
        targetPlanet = planet;
        targetPoint = targetPlanet.transform;

        if (!path.Contains(planet.star))
        {
            OnHoverStar(planet.star);
        }
        else
        {
            DrawLine(path);
        }

    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        start = null;
        lineRenderer.positionCount = 0;

        if (path.Count > 1)
        {
            for (int i = 0; i < controlledFleets.Count; i++)
            {
                if (!controlledFleets[i].Busy())
                {
                    controlledFleets[i].ClearOrders();
                    controlledFleets[i].AddOrder(new MoveOrder(path[path.Count-1],path[path.Count-1].transform,new List<Star>(path)));
                }
            }
        }

        UnToggleFleetIcons();
        controlledFleets.Clear();
        targetPoint = null;
    }

    void UnToggleFleetIcons()
    {
        for (int i = 0; i < controlledFleets.Count; i++)
        {
            controlledFleets[i].star.starShipManager.iconHandler.FindIcon(controlledFleets[i].iconHandlerID).setToggleOn(false);
        }
    }
}
