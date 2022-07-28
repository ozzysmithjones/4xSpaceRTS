using System.Collections.Generic;
using UnityEngine;


public enum DefenceType { SHIELDS, ARMOR, EVASION };


public class SpaceShip : MonoBehaviour
{
    //initialisation
    public Renderer rend;
    protected Fleet fleet;
    private bool Initialised = false;

    //targeting.
    public Transform target;
    private float angleCalculationTimer = 0f;
    protected float targetAngle;

    //movement progress
    public bool pointing = false;

    //movement
    public float speed = 3f;
    public float rotationSpeed = 60f;

    //combat:
    protected bool conflict = false;
    public DefenceType defenceType = DefenceType.EVASION;
    public float hitPoints = 10f;
    public float damage = 1.0f;

    //cargo:
    protected Resources cargo = new Resources();

    void Start()
    {
        if (!Initialised)
        {
            Initialise(Color.white);
        }
    }

    public void SetFleet(Fleet fleet)
    {
        this.fleet = fleet;
    }

    public virtual void Initialise(Color flagColor)
    {
        Initialised = true;
        rend = GetComponent<Renderer>();
        fleet = GetComponentInParent<Fleet>();

        SpriteRenderer spriteRenderer = rend as SpriteRenderer;
        spriteRenderer.color = flagColor;
    }


    public virtual void SetVisibility(bool visible)
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        rend.enabled = visible;
    }

    public virtual void GiveCargo(int faction)
    {
        Master.instance.characters.empires[faction].Gather(cargo);

        for (int i = 0; i < cargo.amounts.Length; i++)
        {
            cargo.amounts[i] = 0;
        }

    }

    public virtual void StartFighting()
    {

    }

    public virtual void Fight(List<SpaceShip> enemies)
    {

    }

    public virtual void StopFighting()
    {

    }

    public void SetAngle(float _angle)
    {
        transform.eulerAngles = new Vector3(0, 0, PositiveAngle(_angle));
    }

    public void Move()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    protected void RotateTowards(Transform target)
    {
        if (target == null)
        {
            return;
        }

        angleCalculationTimer += Time.deltaTime;

        if (angleCalculationTimer >= 0.2f)
        {
            angleCalculationTimer = 0f;
            UpdateTargetAngle(transform.position, target.position);
        }

        RotateTowards(targetAngle);
    }


    public void RotateTowards(float angle)
    {
        //work out the direction we need to rotate(clockwise or anti-clockwise):
        float clock = FindRotationDirection(transform.eulerAngles.z, angle);

        if (clock != 0f)
        {
            if (pointing)
            {
                pointing = false;
            }

            transform.Rotate(Vector3.forward * clock * Time.deltaTime * rotationSpeed);
        }
        else if (!pointing)
        {
            //transform.Rotate(Vector3.forward * clock * Time.deltaTime * rotationSpeed);
            pointing = true;
        }
    }


    protected void UpdateTargetAngle(Vector2 origin, Vector2 target)
    {
        //work out the target angle(I use trigonometry).
        Vector2 difference = (origin - target).normalized;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(difference.y, difference.x));
        angle += 90f;

        targetAngle = angle;
    }

    protected void UpdateTargetAngle()
    {
        UpdateTargetAngle(transform.position, target.position);
    }


    private float FindRotationDirection(float angle, float targetAngle, float maxProximity = 3f)
    {
        float difference = PositiveAngle(targetAngle) - PositiveAngle(angle);


        if (Mathf.Abs(difference) < maxProximity || difference == 0f)
        {
            return 0f;
        }

        if (difference > 180f || (difference < 0f && difference > -180f))
        {
            return -1f;
        }
        else if (difference < -180f || (difference > 0f && difference < 180f))
        {

            return 1f;
        }

        return 1f;
    }

    private float PositiveAngle(float angle)
    {
        if (angle >= 0f)
        {
            return angle;
        }
        else
        {
            return 360f - Mathf.Abs(angle);
        }
    }



    //works out the rock paper and scissors like advantage for an attack vs a defence
    protected float Vantage(WeaponType weaponType, DefenceType defenceType, float advantage = 0.3f, float disadvantage = -0.3f)
    {
        //order of weapons: laser, explosive, slugthrower.
        //order of defences: shield, armor, evasion.

        int attack = (int)weaponType;
        int defence = (int)defenceType;


        if (attack == defence)
        {
            return advantage;
        }
        else if (attack == defence - 1 || (attack == 2 && defence == 0))
        {
            return disadvantage;
        }
        else if (attack == defence + 1 || (attack == 0 && defence == 2))
        {
            return 0.0f;
        }

        return 0.0f;
    }

    public void TakeDamage(SpaceShip aggressor, WeaponType weaponType, float amount)
    {
        hitPoints -= amount * (1f + Vantage(weaponType, defenceType));

        //when there is a death, remove the ship from the fleet and play a cool animation.
        if (hitPoints <= 0f)
        {
            if (aggressor.target == transform)
            {
                aggressor.target = null;
            }

            Explode();
        }
    }

    public virtual void Explode()
    {
        fleet.RemoveShip(this);

        //should put a delay on this if an animation is played.
        Destroy(gameObject);
    }
}
