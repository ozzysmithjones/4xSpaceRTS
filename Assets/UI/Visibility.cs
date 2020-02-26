using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visibility : MonoBehaviour
{
    
    //private Renderer[] renderers;
    private List<Renderer> renderers = new List<Renderer>();
    private List<Image> images = new List<Image>();
    public bool visibility = false;


    private void Start()
   {
        //SetVisibility(false);
        //SetVisibleObjects();
   }
    
   public void SetVisibleObjects()
   {
        //lines = GetComponentsInChildren<LineRenderer>();
        renderers.AddRange(GetComponentsInChildren<Renderer>(false));
        images.AddRange(GetComponentsInChildren<Image>(false));
   }


    public virtual void Initialise()
    {
        SetVisibleObjects();
    }

   public virtual void SetVisibility(bool visible)
   {

        if (visibility == visible)
        {
            return;
        }
        visibility = visible;
        
        for (int i = 0; i < renderers.Count; i++)
        {
            if(renderers[i] == null)
            {
                continue;
            }
            if (renderers[i].enabled != visible)
            {
                renderers[i].enabled = visible;
            }
        }
        for (int i = 0; i < images.Count; i++)
        {
            images[i].enabled = visible;
        }

    }

   public void AddStaticObject(GameObject thing)
    {
        Renderer[] rends = thing.GetComponentsInParent<Renderer>();
        Image[] ims = thing.GetComponentsInChildren<Image>();

        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i].enabled != visibility)
            {
                renderers[i].enabled = visibility;
            }
        }
        for (int i = 0; i < ims.Length; i++)
        {
            ims[i].enabled = visibility;
        }

        renderers.AddRange(rends);
        images.AddRange(ims);
    }

    

    


 


}
