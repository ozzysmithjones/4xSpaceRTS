using UnityEngine;

public enum WeaponType { laser, explosive, slugthrower }

public class Weapon : MonoBehaviour
{
    public bool shooting = false;

    public float range = 10f;
    protected float damage = 0.0f;

    protected Fighter spaceShip;

    private ParticleSystem shootEffect;


    public WeaponType weaponType = WeaponType.laser;

    protected Transform hitTransform;
    protected SpaceShip hitEnemy;

    // Start is called before the first frame update
    void Start()
    {
        spaceShip = GetComponentInParent<Fighter>();
        shootEffect = GetComponent<ParticleSystem>();
        ChildInitialise();
    }

    // Update is called once per frame
    void Update()
    {

    }




    protected virtual void ChildInitialise()
    {


    }

    public virtual void Shoot(float damage = 1f)
    {
        this.damage = damage;
        shooting = true;
        shootEffect.Play();


    }


    public virtual void StopShooting()
    {

        shooting = false;
        shootEffect.Stop();
    }

    protected void DealDamage(float amount, Transform target)
    {

        if (target != hitTransform)
        {

            SpaceShip enemyShip = target.GetComponent<SpaceShip>();
            if (enemyShip != null)
            {
                enemyShip.TakeDamage(spaceShip, weaponType, amount);
                hitTransform = target;
                hitEnemy = enemyShip;
            }
        }
        else
        {
            hitEnemy.TakeDamage(spaceShip, weaponType, amount);

        }
    }
}
