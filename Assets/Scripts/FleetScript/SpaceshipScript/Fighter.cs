using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : SpaceShip
{
    //combat: 

    //slugthrowers beat evasion, but they are weaker agaisnt shields. 
    //rays beat shields, but they are weaker agaisnt armour.
    //explosives beat armour, but they are weaker agaisnt evasion.

    public SpaceShip enemyShip;
    public bool targetAquired = false;

    private Timer targetTimer;
    private Timer targetAngleTimer;

    public Weapon[] weapons;
    private bool shooting = false;

    public override void Initialise(Color flagColor)
    {
        base.Initialise(flagColor);
        targetTimer = new Timer(3f, FindTarget);
        targetAngleTimer = new Timer(0.2f, UpdateTargetAngle);
    }


    //aproach enemy, 
    public override void Fight()
    {
        base.Fight();

        //find a target to shoot at.
        if (target == null)
        {
            FindTarget();
        }

        targetTimer.Tick(Time.deltaTime);
        
        //move towards them.
        FightMove();


        //fire if within a certain range
        FightAttack();

    }

    protected virtual void FightMove()
    {
        Move();
        RotateTowardsTarget();

    }

    protected virtual void FightAttack()
    {
        if (shooting)
        {
            return;
        }
        if(Mathf.DeltaAngle(targetAngle,transform.eulerAngles.z) < 50f)
        {
            if (!shooting)
            {
                shooting = true;
                for (int i = 0; i < weapons.Length; i++)
                {
                    weapons[i].Shoot();
                }
            }    

        }
        else if(shooting)
        {
            shooting = false;
            for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i].StopShooting();
            }
        }
        
    }


    private void FindTarget()
    {
        float shortestDistance = 0.0f;
        int index = 0;
        for(int i = 0; i < fleet.navigatorCombat.target.spaceShips.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, fleet.navigatorCombat.target.spaceShips[i].transform.position);

            if(distance < shortestDistance || i == 0)
            {
                shortestDistance = distance;
                index = i;
            }

        }
        enemyShip = fleet.navigatorCombat.target.spaceShips[index];
        target = enemyShip.transform;
    }

    protected override void OnConflictChange()
    {
        base.OnConflictChange();
        if(!conflict && shooting)
        {
            shooting = false;
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].StopShooting();
            }
        }
    }









}
