using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate float InfluenceFunc(int x, int y);

public abstract class InfluenceTemplate
{
    public int width, height;
    [HideInInspector] public float[] values;

    public InfluenceTemplate(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.values = new float[width * height];
    }

    public void Init()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                values[y * width + x] = Calculate(x, y);
            }
        }
    }

    protected abstract float Calculate(int x, int y);
}

public class InfluenceMap
{
    private int width, height;
    private float[] values;

    public InfluenceMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.values = new float[width * height];
    }

    public float this[int x, int y]
    {
        get
        {
            return values[y * width + x];
        }

        set
        {
            values[y * width + x] = value;
        }
    }

    public void Clear()
    {
        for(int i = 0; i < values.Length; ++i)
        {
            values[i] = 0.0f;
        }
    }

    public void Propagate(int x, int y, InfluenceTemplate influenceTemplate)
    {
        for(int yi = 0; yi < influenceTemplate.height; ++yi)
        {
            for (int xi = 0; xi < influenceTemplate.width; ++xi)
            {
                values[(yi + y) * width + (xi + x)] = influenceTemplate.values[yi * influenceTemplate.width + xi];
            }
        }
    }

    public void Propagate(int x, int y, int width, int height, InfluenceFunc influenceFunc)
    {
        for (int yi = 0; yi < height; ++yi)
        {
            for (int xi = 0; xi < width; ++xi)
            {
                values[(yi + y) * this.width + (xi + x)] = influenceFunc(xi, yi);
            }
        }
    }

    public void PropagateByStar(Star origin, int depth, InfluenceFunc influenceFunc)
    {
        List<Star> stars = Master.instance.Presence(origin, depth);

        foreach(Star star in stars)
        {
            values[star.y * width + star.x] = influenceFunc(star.x, star.y);
        }
    }
}


