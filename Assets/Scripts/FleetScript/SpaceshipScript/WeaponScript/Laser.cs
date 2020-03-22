using System.Collections;
using UnityEngine;

public class Laser : Weapon
{
    private float fireRate = 60f;
    private LineRenderer beam;

    // Start is called before the first frame update
    protected override void ChildInitialise()
    {
        beam = GetComponent<LineRenderer>();
        beam.useWorldSpace = false;
        beam.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
    }

    public override void Shoot(float damage = 1.0f)
    {
        base.Shoot(damage);

        StartCoroutine(ConstantBeam());
    }

    IEnumerator ConstantBeam()
    {
        beam.enabled = true;
        while (shooting)
        {
            if (spaceShip.target != null)
            {
                beam.SetPosition(1, transform.InverseTransformPoint(spaceShip.target.position));
                //transform.up = (spaceShip.target.position - transform.position).normalized;

                //hit damage.
                DealDamage(damage * 1f / fireRate, spaceShip.target);
            }
            else
            {
                beam.SetPosition(1, Vector2.up * range);
            }


            yield return new WaitForSeconds(1f / fireRate);
        }

        beam.enabled = false;

        yield return null;
    }


}
