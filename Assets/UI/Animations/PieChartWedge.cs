using UnityEngine;
using UnityEngine.UI;
public class PieChartWedge : MonoBehaviour
{
    public Image wedgeImage;

    public int index = -1;
    public float startAngle = 0.0f;
    public float endAngle = 0.0f;

    public void SetColor(Color color)
    {
        wedgeImage.color = color;
    }
    public void SetStartAngle(float startAngle)
    {
        transform.eulerAngles = new Vector3(0, 0, startAngle);

    }
    public float GetStartAngle()
    {
        return transform.eulerAngles.z;// Calculation.WrapAngle(transform.eulerAngles.z); ;
    }
    public void SetSize(float size)
    {
        wedgeImage.fillAmount = size;
    }
    public void SetEndAngle(float endAngle)
    {
        wedgeImage.fillAmount = (endAngle - transform.eulerAngles.z) / 360.0f;
    }
    public float GetEndAngle()
    {
        float angle = GetStartAngle() + (wedgeImage.fillAmount * 360.0f);
        if (angle >= 360.0f)
        {
            angle = 360.0f;
        }
        return angle;


    }


}
