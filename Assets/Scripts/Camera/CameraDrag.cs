using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Camera cam;
    private Vector3 origin;
    private Vector3 difference;
    private bool dragging = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            if (!dragging)
            {
                dragging = true;
                origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            difference = cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position;
        }
        else
        {
            dragging = false;
        }

        if(dragging)
        {
            cam.transform.position = origin - difference;
        }
    }
}
