using UnityEngine;
using UnityEngine.EventSystems;

public class WedgeMouseInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    PieChart pieChart;

    public delegate void OnWedgeInput(PieChartWedge pieChartWedge);
    public event OnWedgeInput ClickWedge;

    public event OnWedgeInput OnWedgeEnter;
    public event OnWedgeInput OnWedgeExit;

    private PieChartWedge current;

    private void Start()
    {
        rectTransform = transform as RectTransform;
        pieChart = GetComponent<PieChart>();
    }

    public void AddToMouseClickEvent(OnWedgeInput onClickWedge, bool listening)
    {
        if (listening)
        {
            ClickWedge += onClickWedge;
        }
        else
        {
            ClickWedge -= onClickWedge;
        }
    }
    public void AddToMouseEnterEvent(OnWedgeInput onMouseEnterWedge, bool listening)
    {
        if (listening)
        {
            OnWedgeEnter += onMouseEnterWedge;
        }
        else
        {
            OnWedgeEnter -= onMouseEnterWedge;
        }
    }
    public void AddToMouseExitEvent(OnWedgeInput onMouseExitWedge, bool listening)
    {
        if (listening)
        {
            OnWedgeExit += onMouseExitWedge;
        }
        else
        {
            OnWedgeExit -= onMouseExitWedge;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 diff = eventData.position - (Vector2)rectTransform.position;
        float angle = VectorToPieAngle(diff);

        for (int i = 0; i < pieChart.wedges.Length; i++)
        {
            if (pieChart.wedges[i].wedgeImage.fillAmount <= 0.0f)
            {
                continue;
            }
            float startAngle = pieChart.wedges[i].GetStartAngle();
            float endAngle = pieChart.wedges[i].GetEndAngle();

            if (angle > startAngle && angle < endAngle)
            {
                current = pieChart.wedges[i];
                if (OnWedgeEnter != null)
                {
                    OnWedgeEnter.Invoke(current);
                }
                break;
            }

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnWedgeExit != null)
        {
            OnWedgeExit(current);
        }
        current = null;
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if(current == null)
        {
            return;
        }
        ClickWedge(current);

    }

    private static float VectorToPieAngle(Vector2 diff)
    {
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        angle += 90.0f;

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
