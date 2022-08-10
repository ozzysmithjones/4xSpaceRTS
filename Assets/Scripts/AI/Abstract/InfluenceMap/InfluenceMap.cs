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
    public int Length { get => values.Length; }
    private readonly double[] values;

    public InfluenceMap()
    {
        this.values = null;
    }

    public InfluenceMap(int numValues)
    {
        this.values = new double[numValues];
    }

    public double this[int index]
    {
        get
        {
            return values[index];
        }
        set
        {
            values[index] = value;
        }
    }

    public void Clear()
    {
        for(int i = 0; i < values.Length; ++i)
        {
            values[i] = 0.0f;
        }
    }

    public void PropagateByStar(Star origin, int depth, StarFallOffFunc influenceFunc)
    {
        List<Star> stars = Master.instance.Presence(origin, depth);

        foreach(Star star in stars)
        {
            values[star.index] = influenceFunc(star);
        }
    }
}

public class HierarchicalInfluenceMap
{
    private InfluenceMap[] influenceMaps;

    public HierarchicalInfluenceMap(int numInfluenceMaps, int size)
    {
        this.influenceMaps = new InfluenceMap[numInfluenceMaps];

        for (int i = 0; i < numInfluenceMaps; ++i)
        {
            this.influenceMaps[i] = new InfluenceMap(size);
        }
    }

    public void Clear()
    {
        for(int i = 0; i < influenceMaps.Length; ++i)
        {
            influenceMaps[i].Clear();
        }
    }

    public InfluenceMap this[int layer]
    {
        get
        {
            return influenceMaps[layer];
        }
    }

    public InfluenceMap GetInfluenceMap(int layer)
    {
        return influenceMaps[layer];
    }
}
