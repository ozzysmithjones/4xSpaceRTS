using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorCombat : MonoBehaviour
{
    private Navigator navigator;
    private List<Navigator> enemies = new List<Navigator>();
    private bool conflict = false;

    public Navigator target;


    private void Awake()
    {
        navigator = GetComponent<Navigator>();
    }


    public void UpdateTarget()
    {
        float shortestDistance = 0.0f;
        for(int i = 0; i < enemies.Count; i++)
        {
            //prioritises those fighting it.
            if(enemies[i].navigatorCombat.target == navigator)
            {
                target = enemies[i];
                break;
            }
            //otherwise closest:
            float dist = Vector2.Distance(navigator.center.position, enemies[i].center.position);
            if (dist < shortestDistance || i == 0)
            {
                shortestDistance = dist;
                target = enemies[i];
                
            }
        }

        if(target == null)
        {
            SetConflict(false);
        }
       

    }

    public bool IsConflict(List<Navigator> fleets)
    {
        bool newConflict = false;
        for(int i = 0; i < fleets.Count; i++)
        {
            if(fleets[i].faction != navigator.faction)
            {
                newConflict = true;
                enemies.Add(fleets[i]);
            }

        }

        SetConflict(newConflict);

        return conflict;
    }

    public bool AddEnemy(Navigator possibleEnemy)
    {
        if(navigator.faction != possibleEnemy.faction)
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
    public bool RemoveEnemy(Navigator possibleEnemy)
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
        if(conflict == value)
        {
            return;
        }

        if(value)
        {
            UpdateTarget();
        }
        else
        {
            target = null;
        }

        conflict = value;

        //could be used to force the fleet to flee, or something instead. By default sets the spaceships to be in conflict.
        navigator.ConflictReaction(conflict);

    }
        
        

}
