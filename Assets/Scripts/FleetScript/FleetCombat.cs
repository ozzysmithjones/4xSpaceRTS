using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetCombat : MonoBehaviour
{
    private bool conflict;
    private Fleet fleet;
    private List<Fleet> enemies = new List<Fleet>();

    public Fleet target;


    // Start is called before the first frame update
    void Awake()
    {
        fleet = GetComponent<Fleet>();
    }

    public void UpdateTarget()
    {
        float shortestDistance = 0.0f;
        for (int i = 0; i < enemies.Count; i++)
        {
            //prioritises those fighting it.
            if (enemies[i].fleetCombat.target == this.fleet)
            {
                target = enemies[i];
                break;
            }
            //otherwise closest:
            float dist = Vector2.Distance(fleet.center.position, enemies[i].center.position);
            if (dist < shortestDistance || i == 0)
            {
                shortestDistance = dist;
                target = enemies[i];

            }
        }

        if (target == null)
        {
            SetConflict(false);
        }


    }

    public bool IsConflict(List<Fleet> fleets)
    {
        bool newConflict = false;
        for (int i = 0; i < fleets.Count; i++)
        {
            if (fleets[i].faction != fleet.faction)
            {
                newConflict = true;
                enemies.Add(fleets[i]);
            }

        }
        SetConflict(newConflict);
        return conflict;
    }

    public bool AddEnemy(Fleet possibleEnemy)
    {
        if (fleet.faction != possibleEnemy.faction)
        {
            if (!enemies.Contains(possibleEnemy))
            {
                enemies.Add(possibleEnemy);

                if (!conflict)
                {
                    SetConflict(true);
                }
                return true;
            }

        }

        return false;
    }
    public bool RemoveEnemy(Fleet possibleEnemy)
    {

        if (enemies.Contains(possibleEnemy))
        {
            enemies.Remove(possibleEnemy);
            if (possibleEnemy == target)
            {
                UpdateTarget();
            }
            return true;
        }
        if (enemies.Count <= 0)
        {
            SetConflict(false);
        }

        return false;
    }

    void SetConflict(bool value)
    {
        if (conflict == value)
        {
            return;
        }

        if (value)
        {
            UpdateTarget();
        }
        else
        {
            target = null;
        }

        conflict = value;

        //could be used to force the fleet to flee, or something instead. By default sets the spaceships to be in conflict.
        fleet.ReactToConflict(conflict);

    }
}
