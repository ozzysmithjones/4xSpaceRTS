using UnityEngine;
using UnityEngine.EventSystems;

public class WedgePress : MonoBehaviour, IPointerDownHandler
{
    RectTransform rectTransform;
    PieChart pieChart;

    public delegate void OnClickWedge(PieChartWedge pieChartWedge);
    public event OnClickWedge ClickWedge;

    private void Start()
    {
        rectTransform = transform as RectTransform;
        pieChart = GetComponent<PieChart>();
    }

    public void AddToClickEvent(OnClickWedge onClickWedge)
    {
        ClickWedge += onClickWedge;
    }
    public void RemoveFromClickEvent(OnClickWedge onClickWedge)
    {
        ClickWedge -= onClickWedge;
    }

    public void OnPointerDown(PointerEventData eventData)
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
                ClickWedge.Invoke(pieChart.wedges[i]);
                break;
            }

        }

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

    private float AngleDifference(float a, float b)
    {
        float diff = b - a;

        return Calculation.WrapAngle(diff);

    }
}
