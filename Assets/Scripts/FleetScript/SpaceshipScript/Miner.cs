using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Drone
{

    private Animator animator;

    private float alpha = 0.0f;
    private float distance = 0.0f;
    private Transform planet;

    
    public enum Process { APROACH, MINE, RETURN, DOCKED};
    public Process process = Process.DOCKED;




    public override void Initialise(Color flagColor)
    {
        animator = GetComponent<Animator>();
        base.Initialise(flagColor);

       
    }

    public override void OnPoint(Transform goal)
    {
        base.OnPoint(goal);

        distance = Vector2.Distance(transform.position, goal.position);
    }

    public override void OnSetPath(Transform goal)
    {
        base.OnSetPath(goal);
        if (process == Process.DOCKED)
        {
            process = Process.APROACH;
        }
        else
        {
            process = Process.RETURN;
        }
    }


    public override void PathUpdate()
    {

        
        if (!pointing || conflict)
        {
            base.PathUpdate();
        }
        else
        {
            
            alpha += Time.deltaTime * (speed / distance);

            transform.Translate(Vector3.up * Time.deltaTime * speed);

            if(alpha >= 1f)
            {
                alpha = 0.0f;
                if (process == Process.APROACH)
                {
                    //StartCoroutine(Mine());
                    
                    ClearPath();
                    if(animator == null)
                    {
                        Debug.LogError("Animator is null");
                        animator = GetComponent<Animator>();
                    }
                    animator.SetBool("Mining", true);
                    process = Process.MINE;

                    shipCarrier.OnReach(this);
                    Invoke("ReturnToDock", 4f);

                }
                else if(process == Process.RETURN)
                {
                    //dock the ship. 
                   
                   
                    shipCarrier.Dock(this);
                    process = Process.DOCKED;
                }

            }
        }
        
    }

    IEnumerator Mine(float miningTime = 4f)
    {
        ClearPath();
        animator.SetBool("Mining", true);
        process = Process.MINE;

        yield return new WaitForSeconds(miningTime);

    }

    void ReturnToDock()
    {
        cargo[1] = 1;

        animator.SetBool("Mining", false);

        SetPath(dock);
        alpha = 0.0f;
        process = Process.RETURN;
    }

    public override void OnDock()
    {
        ClearPath();
        process = Process.DOCKED;
    }
}
