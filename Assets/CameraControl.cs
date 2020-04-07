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

    private Camera cam;

    public bool visible = false;
    public float visibilityMaxSize = 20f;
    public float VisibilityMaxDistance = 20f;
    public float visibilityRadius = 20f;

    private Star starBeingViewed = null;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        size = cam.orthographicSize;
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
    }

    void Movement()
    {
        float zoomBonus = Mathf.InverseLerp(1f, 20f, (float)zoomStep);
        zoomBonus = Mathf.Clamp(zoomBonus, 0.05f, zoomBonus);
        transform.Translate(new Vector3(horizontal, vertical, 0f) * Time.fixedDeltaTime * panSpeed * zoomBonus);

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
            size = Mathf.Pow((float)zoomStep, 2f);

            cam.orthographicSize = size;

            ZoomVisibility();
        }
    }

    void ZoomVisibility()
    {
        if (size < visibilityMaxSize && !visible)
        {
            Show();

        }
        else if (size > visibilityMaxSize && visible)
        {
            Hide();
        }
    }

    void Show()
    {

        visible = true;
        Master.instance.userInterface.SetMapUI(false);

        Vector3 center = new Vector3(transform.position.x, transform.position.y, 0);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(center, visibilityRadius, Vector3.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            Star star = hits[i].collider.GetComponent<Star>();
            if (star != null)
            {
                starBeingViewed = star;
                starBeingViewed.starUI.SetUIVisibility(false);
                return;
            }
        }
    }




    void Hide()
    {
        
        visible = false;
        Master.instance.userInterface.SetMapUI(true);
        if (starBeingViewed != null)
            starBeingViewed.starUI.SetUIVisibility(true);
    }





}
