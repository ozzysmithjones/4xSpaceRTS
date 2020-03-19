using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool 
{

    public virtual void OnSelected()
    {
       
    }

    public virtual void OnDeselected()
    {

    }

    public virtual void OnInteract(Star star)
    {

    }

    public virtual void OnHover(Star star)
    {

    }
}
