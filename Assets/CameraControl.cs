using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float maxScrollSpeed = 700f;
    public float minScrollSpeed = 100f;
    public float panSpeed = 6f;


    private float horizontal = 0f;
    private float vertical = 0f;
    private float zoom = 0f;
    public int zoomStep = 0;

    private float size = 0;
    
    //private float minSize = 5f;
   // private float maxSize = 500f;

    private Camera cam;

    public bool visible = false;
    
    private List<Collider2D> viewables = new List<Collider2D>();
    public float visibilityMaxSize = 20f;
    public float VisibilityMaxDistance = 20f;
    public float visibilityRadius = 20f;

   

    private Vector3 viewPosition;
    private float viewDistanceAlpha = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        size = cam.orthographicSize;
        StartCoroutine(ZoomVisibility(0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        zoom = Input.GetAxis("Mouse ScrollWheel");
    }

    private void FixedUpdate()
    {
        Movement();
        Zoom();
        //cam.orthographicSize = cam.pixelWidth / (((cam.pixelWidth / cam.pixelHeight) * 2f) * Mathf.Pow((float)zoomStep,2f));
        //ZoomVisibility();
    }

    void Movement()
    {
        float zoomBonus = Mathf.InverseLerp(1f, 20f, (float)zoomStep);
        zoomBonus = Mathf.Clamp(zoomBonus,0.05f, zoomBonus);
        //panning the camera.
        transform.Translate(new Vector3(horizontal, vertical, 0f) * Time.fixedDeltaTime * panSpeed * zoomBonus);
        //HideFarAwayStars();
        //ShowCloseStars();
       
    }

    void Zoom()
    {
        if (zoom != 0)
        {
          if (zoom > 0)
            {
                zoomStep -= 1;
            }
            else
            {
                zoomStep += 1;
            }
          zoomStep = Mathf.Clamp(zoomStep, 1, 30);
          size = Mathf.Pow((float)zoomStep,2f);

           cam.orthographicSize = size;
            
        // 
         //ZoomVisibility();

        }
    }
    //finds out if a value has decimals.
    bool Decimals(float value)
    {
        if (value % 1f == 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ZoomVisibility(float wait = 1f)
    {
      
        while (true)
        {
            yield return new WaitForSeconds(wait);

            if (size < visibilityMaxSize)
            {
                Hide();
                Show();

            }
            else if (size > visibilityMaxSize && visible)
            {
                Hide();
                
            }

        }
        
       
    }

    void Show()
    {
        viewables.Clear();

        visible = true;
        Vector3 center = new Vector3(transform.position.x, transform.position.y, 0);
        viewPosition = center;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(center, visibilityRadius, Vector3.zero);

        Master.instance.userInterface.SetMapUI(false);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.GetComponent<Star>() != null)
            {
                Visibility visibility = hits[i].collider.GetComponent<Visibility>();
                if (visibility != null)
                {
                    viewables.Add(hits[i].collider);
                    //visibility.ZoomVisibility(visibility.transform, true);
                    
                    hits[i].collider.enabled = false;
                }
            }
        }
    }




    void Hide()
    {
        viewDistanceAlpha = 0f;
        visible = false;
        Master.instance.userInterface.SetMapUI(true);
        for (int i = 0; i < viewables.Count; i++)
        {
            
            viewables[i].enabled = true;
            

        }

    }

    

    void HideFarAwayStars()
    {
        if (visible)
        {
            viewDistanceAlpha += Time.fixedDeltaTime;
            if(viewDistanceAlpha >= 0.5f)
            {
                viewDistanceAlpha = 0f;
                float distance = Vector2.Distance(new Vector3(transform.position.x, transform.position.y, 0), viewPosition);

                if (distance > VisibilityMaxDistance)
                {
                    Hide();
                }

            }
        }
    }


    void ShowCloseStars()
    {
        if (size < visibilityMaxSize)
        {
            viewDistanceAlpha += Time.fixedDeltaTime;
            if (viewDistanceAlpha >= 0.5f)
            {
                viewDistanceAlpha = 0f;
                Show();

            }
        }



    }

    
   



}
