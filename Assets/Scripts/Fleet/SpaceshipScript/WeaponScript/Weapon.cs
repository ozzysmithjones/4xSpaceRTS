using UnityEngine;

public enum WeaponType { laser, explosive, slugthrower }

public abstract class Weapon : MonoBehaviour
{
    public bool shooting = false;
    protected float damage = 1.0f;

    protected SpaceShip spaceShip;
    public WeaponType weaponType = WeaponType.laser;

    protected Transform hitTransform;
    protected SpaceShip hitEnemy;

    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        spaceShip = GetComponentInParent<SpaceShip>();
        Init();
    }

    protected abstract void Init();

    public abstract void StartShooting();

    public abstract void Shoot(SpaceShip enemy);

    public abstract void StopShooting();

    protected void DealDamage(SpaceShip enemy, float damage)
    {
        enemy.TakeDamage(spaceShip, weaponType, damage);
    }
}
