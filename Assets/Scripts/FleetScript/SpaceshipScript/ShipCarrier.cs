using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCarrier : SpaceShip
{
    public List<Drone> drones = new List<Drone>();
    public int unDockedDrones = 0;


    public IEnumerator WaitForDrones()
    {
        while(unDockedDrones > 0)
        {
            yield return new WaitForSeconds(3f);
        }

    }

    public void SetDronePaths(List<Transform> paths, bool useAllDrones = false)
    {
        if (paths.Count <= 0)
        {
            return;
        }
        if (useAllDrones)
        {
            
            int pathIndex = 0;

            for(int i = 0; i < drones.Count; i++)
            {
                
                drones[i].shipCarrier = this;

                drones[i].SetPath(paths[pathIndex]);
                drones[i].transform.SetParent(transform.parent);
                unDockedDrones++;

                pathIndex++;
                
                if(pathIndex >= paths.Count)
                {
                    pathIndex = 0;
                }
               
            }
            return;
        }


        for(int i = 0; i < paths.Count; i++)
        {
            if (i >= drones.Count)
            {
                return;
            }


            drones[i].shipCarrier = this;

            drones[i].SetPath(paths[i]);
            drones[i].transform.SetParent(transform.parent);
            unDockedDrones++;
           
        }
    }


    public void SetDronePath(Transform path)
    {
        drones[drones.Count - 1].SetPath(path);
    }

    public void DockAll()
    {
        for(int i = 0; i < drones.Count; i++)
        {
            Dock(drones[i]);
        }
    }

    private void UnDock(Drone drone)
    {
        drone.shipCarrier = this;
        drone.transform.SetParent(transform.parent);
        unDockedDrones++;
    }
    

    public void Dock(Drone drone)
    {
        drone.OnDock();
        drone.ResetTransform();
        unDockedDrones--;
    }

    public override void SetVisibility(bool visible)
    {
        for(int i = 0; i < drones.Count; i++)
        {
            drones[i].SetVisibility(visible);
        }
    }

    public override void GiveCargo(int faction)
    {
        base.GiveCargo(faction);

        for(int i = 0; i < drones.Count; i++)
        {
            drones[i].GiveCargo(faction);
        }
    }

    public virtual void OnReach(Drone drone)
    {

    }

    public void SetDronesPath(Transform transform)
    {
        for (int i = 0; i < drones.Count; i++)
        {
            drones[i].shipCarrier = this;

            drones[i].SetPath(transform);
            drones[i].transform.SetParent(transform.parent);
            unDockedDrones++;
        }

    }

    protected override void OnConflictChange()
    {
        base.OnConflictChange();

        if (conflict)
        {
            for (int i = 0; i < drones.Count; i++)
            {
                UnDock(drones[i]);
                drones[i].SetConflict(conflict);
            }
        }
        else
        {
            for (int i = 0; i < drones.Count; i++)
            {
                drones[i].SetConflict(conflict);
            }
            DockAll();
        }



    }


}
