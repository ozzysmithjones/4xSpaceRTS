using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WedgePress : MonoBehaviour, IPointerDownHandler
{
    RectTransform rectTransform;
    PieChartWedge pieChartWedge;

    private void Start()
    {
        rectTransform = transform as RectTransform;
        pieChartWedge = GetComponent<PieChartWedge>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 diff = eventData.position - (Vector2)rectTransform.position;
        float angle = VectorToPieAngle(diff);

        if(angle < pieChartWedge.GetStartAngle() && angle > pieChartWedge.GetEndAngle())
        {
            Debug.Log("hit " + name);
        }
    }

    private static float VectorToPieAngle(Vector2 diff)
    {
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        angle *= -1;
        angle -= 90.0f;

        if (angle < 0.0f)
        {
            angle = 360 + angle;
        }
        if (angle > 360.0f)
        {
            angle %= 360f;
        }

        return angle;
    }
}
