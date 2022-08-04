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
        targetPlanet = null;
    }

    void DrawLine(List<Star> linePath)
    {
        lineRenderer.positionCount = linePath.Count + (targetPlanet != null ? 1 : 0);
        for (int i = 0; i < linePath.Count; i++)
        {
            lineRenderer.SetPosition(i, linePath[i].transform.position + new Vector3(0f, 0f, 3f));
        }

        if(targetPlanet != null)
        {
            lineRenderer.SetPosition(linePath.Count, targetPlanet.transform.position + new Vector3(0f, 0f, 3f));
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
        Transform point = targetPlanet != null ? targetPlanet.transform : path[path.Count - 1].transform;

        if (path.Count > 1)
        {
            for (int i = 0; i < controlledFleets.Count; i++)
            {
                if (!controlledFleets[i].Busy())
                {
                    IOrder order;
                    if (targetPlanet != null && controlledFleets[i].type == FleetType.Colony)
                    {
                        order = new ColoniseOrder(targetPlanet, new List<Star>(path));
                    }
                    else
                    {
                        order = new MoveOrder(path[path.Count - 1], point, new List<Star>(path));
                    }

                    controlledFleets[i].ClearOrders();
                    controlledFleets[i].AddOrder(order);
                }
            }

        }else if(targetPlanet != null)
        {
            for (int i = 0; i < controlledFleets.Count; i++)
            {
                if (!controlledFleets[i].Busy())
                {
                    IOrder order;
                    if (controlledFleets[i].type == FleetType.Colony)
                    {
                        order = new ColoniseOrder(targetPlanet, null);
                    }
                    else
                    {
                        order = new MoveOrder(targetPlanet.star, point, null);
                    }

                    controlledFleets[i].ClearOrders();
                    controlledFleets[i].AddOrder(order);
                }
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
