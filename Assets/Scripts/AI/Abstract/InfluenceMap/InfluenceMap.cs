using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate double FallOffFunc(int x, int y);
public delegate double StarFallOffFunc(Star star);

public abstract class InfluenceTemplate
{
    public int width, height;
    [HideInInspector] public double[] values;

    public InfluenceTemplate(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.values = new double[width * height];
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
    public readonly int width, height;
    private readonly double[] values;

    public InfluenceMap()
    {
        this.width = 0;
        this.height = 0;
        this.values = null;
    }

    public InfluenceMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.values = new double[width * height];
    }

    public double this[int x, int y]
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

    public void Propagate(int x, int y, int width, int height, FallOffFunc influenceFunc)
    {
        for (int yi = 0; yi < height; ++yi)
        {
            for (int xi = 0; xi < width; ++xi)
            {
                values[(yi + y) * this.width + (xi + x)] = influenceFunc(xi, yi);
            }
        }
    }

    public void PropagateByStar(Star origin, int depth, StarFallOffFunc influenceFunc)
    {
        List<Star> stars = Master.instance.Presence(origin, depth);

        foreach(Star star in stars)
        {
            values[star.y * width + star.x] = influenceFunc(star);
        }
    }
}

public class HierarchicalInfluenceMap
{
    public readonly int width, height;
    private InfluenceMap[] influenceMaps;

    public HierarchicalInfluenceMap(int numInfluenceMaps, int width, int height)
    {
        this.width = width;
        this.height = height;
        this.influenceMaps = new InfluenceMap[numInfluenceMaps];

        for (int i = 0; i < numInfluenceMaps; ++i)
        {
            this.influenceMaps[i] = new InfluenceMap(width, height);
        }
    }

    public void Clear()
    {
        for(int i = 0; i < influenceMaps.Length; ++i)
        {
            influenceMaps[i].Clear();
        }
    }

    public double this[int x, int y]
    {
        get
        {
            double value = 0.0f;

            for(int i = 0; i < influenceMaps.Length; ++i)
            {
                value += influenceMaps[i][x, y];
            }

            return value;
        }
    }

    public InfluenceMap GetInfluenceMap(int layer)
    {
        return influenceMaps[layer];
    }
}
