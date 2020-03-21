using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChartWedge : MonoBehaviour
{
    public Image wedgeImage;

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
        if(transform.eulerAngles.z < 0.0f)
        {
            return 360.0f - transform.eulerAngles.z;
        }
        else
        {
            return transform.eulerAngles.z;
        }
        
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
        return GetStartAngle() - (wedgeImage.fillAmount * 360.0f);
    }
}
