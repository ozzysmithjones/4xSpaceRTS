using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float zoomSpeed = 10;
    public float panSpeed = 80;
    private Camera cam;
    private Vector3 move;

    private bool visible = false;
    private Star starBeingViewed = null;
    private float visibilityRadius = 20.0f;
    private float visibilityMaxSize = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        move.z = Input.GetAxis("Mouse ScrollWheel");

        cam.transform.Translate(Time.deltaTime * panSpeed * (Vector2)move);
        cam.orthographicSize -= move.z * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, cam.orthographicSize + 1);
        ZoomVisibility();
    }

    void ZoomVisibility()
    {
        if (cam.orthographicSize < visibilityMaxSize && !visible)
        {
            Show();

        }
        else if (cam.orthographicSize > visibilityMaxSize && visible)
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