using System.Collections;
using UnityEngine;

public class Laser : Weapon
{
    private float timer = 0.0f;
    private const float fireRate = 60f;
    private const float invFireRate = 1.0f / fireRate;

    private LineRenderer beam;

    protected override void Init()
    {
        beam = GetComponent<LineRenderer>();
        beam.useWorldSpace = false;
        beam.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
        beam.enabled = false;
    }

    public override void StartShooting()
    {
        beam.enabled = true;
    }

    public override void Shoot(SpaceShip enemy)
    {
        beam.SetPosition(1, transform.InverseTransformPoint(spaceShip.target.position));

        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            timer = invFireRate;
            DealDamage(enemy, damage / fireRate);
        }
    }

    public override void StopShooting()
    {
        beam.enabled = false;
    }
}
