using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : DogFighter
{
    public Transform dock;
    private Quaternion originalRotation = Quaternion.identity;
    public ShipCarrier shipCarrier;


    private void Awake()
    {
        originalRotation = transform.localRotation;
    }
    public void ResetTransform()
    {
        transform.SetParent(dock);
        transform.localPosition = Vector3.zero;
        transform.localRotation = originalRotation;

        
        ClearPath();
    }

    public virtual void OnDock()
    {

    }
}
