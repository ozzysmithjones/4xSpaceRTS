using System.Collections.Generic;
using UnityEngine;

public class Fighter : SpaceShip
{
    //combat: 

    //slugthrowers beat evasion, but they are weaker agaisnt shields. 
    //rays beat shields, but they are weaker agaisnt armour.
    //explosives beat armour, but they are weaker agaisnt evasion.

    public SpaceShip enemyShip;
    public Weapon[] weapons;
    private bool shooting = false;
    private float targetTimer = 0.0f;
    private const float targetTime = 1.0f;

    public override void Initialise(Color flagColor)
    {
        base.Initialise(flagColor);
    }


    //aproach enemy, 
    public override void Fight(List<SpaceShip> enemies)
    {
        if(enemies.Count <= 0)
        {
            return;
        }

        base.Fight(enemies);

        targetTimer -= Time.deltaTime;
        if(targetTimer <= 0)
        {
            targetTimer = targetTime;
            RandomTarget(enemies);
            //FindTarget(enemies);
        }

        //move towards them.
        Move();
        RotateTowards(target);

        //fire if within a certain range
        Attack();
    }
    public override void StartFighting()
    {
        base.StartFighting();

        for(int i = 0; i < weapons.Length;++i)
        {
            weapons[i].StartShooting();
        }
    }

    public override void StopFighting()
    {
        base.StopFighting();

        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].StopShooting();
        }
    }

    protected virtual void Attack()
    {
        if (enemyShip != null)
        {
            for (int i = 0; i < weapons.Length; ++i)
            {
                weapons[i].Shoot(enemyShip);
            }
        }
    }

    //returns the ship with the lowest health.
    private void FindTarget(List<SpaceShip> enemies)
    {
        float lowestHP = 0.0f;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].hitPoints < lowestHP || i == 0)
            {
                lowestHP = enemies[i].hitPoints;
                enemyShip = enemies[i];
            }
        }

        target = enemyShip.transform;
    }

    private void RandomTarget(List<SpaceShip> enemies)
    {
        enemyShip = enemies[Random.Range(0, enemies.Count)];
        target = enemyShip.transform;
    }

    
}
