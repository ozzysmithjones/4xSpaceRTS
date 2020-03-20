using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChart : MonoBehaviour
{
    public PieChartWedge[] wedges;
    public Color[] colors;
    public float[] values;
    float[] relativeSizes;


    public void Start()
    {
        UpdateWedges(values,colors);
    }

    private float[] RelativeSizes(float[] values)
    {
        float total = 0.0f;
        for(int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }
        if (total <= 0.0f)
        {
            return values;
        }
        float[] relativeSizes = new float[values.Length];
        for(int i = 0; i < values.Length; i++)
        {
            relativeSizes[i] = values[i] / total;
        }
        return relativeSizes;
    }

    private float RelativeSize(int index, float[] values)
    {
        float total = 0.0f;
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }
        if(total <= 0.0f)
        {
            return 0.0f;
        }
        return values[index] / total;
    }


    public void UpdateWedges(float[] values, Color[] colors)
    {
        this.values = values;
        this.colors = colors;
        this.relativeSizes = RelativeSizes(values);

        float lastAngle = 0.0f; 
        for(int i = 0; i < relativeSizes.Length;i++)
        {
            wedges[i].SetStartAngle(lastAngle);
            wedges[i].SetSize(relativeSizes[i]);
            wedges[i].SetColor(colors[i]);
            lastAngle -= relativeSizes[i] * 360f;
        }

    }

    public void UpdateWedge(int wedgeIndex, float value, Color color)
    {
        values[wedgeIndex] = value;
        relativeSizes[wedgeIndex] = RelativeSize(wedgeIndex,values);
        wedges[wedgeIndex].SetColor(color);

        float lastAngle = 0.0f;
        for (int i = 0; i < relativeSizes.Length; i++)
        {
            wedges[i].SetStartAngle(lastAngle);
            wedges[i].SetSize(relativeSizes[i]);
            lastAngle -= relativeSizes[i] * 360f;
        }
    }


}
