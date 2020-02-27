using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeType
{
    BARREN,
    DESERT,
    TOXIC,
    CARBON,
    JUNGLE,
    OCEAN,
    ICE,
    ODD
}
[CreateAssetMenu(fileName = "BiomeGradient", menuName = "Biomes/Gradient")]
public class BiomeGradient  : ScriptableObject
{


    [SerializeField]
    private Point[] timeline;

    [System.Serializable]
    public class Point
    {
        public float time = 0.0f;
        public BiomeType biomeType;
        public Color color;

        public ResourceType resourceType;
        public int resources = 0;
      


        public Point(float time,BiomeType biomeType, Color color, ResourceType resourceType, int resources)
        {
            this.biomeType = biomeType;
            this.time = time;
            this.color = color;

            this.resourceType = resourceType;
            this.resources = resources;
        }

    }


    public BiomeGradient(Point[] points)
    {

        timeline = new Point[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            timeline[i] = points[i];
        }
    }


    public Color GetColor(float alpha)
    {
        Color color;

        if (timeline.Length <= 0)
        {
            return Color.magenta;
        }
        else if (timeline.Length == 1)
        {
            return timeline[0].color;
        }

        for (int i = 0; i < timeline.Length - 1; i++)
        {
            if (timeline[i].time <= alpha && timeline[i + 1].time >= alpha)
            {
                float a = (alpha - timeline[i].time) / (timeline[i+1].time - timeline[i].time); 
                float lerp = Mathf.InverseLerp(timeline[i].time, timeline[i + 1].time, a);
                color = Color.Lerp(timeline[i].color, timeline[i + 1].color, lerp);
                return color;
            }
        }

        return timeline[timeline.Length - 1].color;
    }

    public BiomeGradient.Point GetPoint(float alpha)
    {
        Color color;

        if (timeline.Length <= 0)
        {
            Debug.LogError("biome gradient array is not storing anything");
            return null;
        }
        else if (timeline.Length == 1)
        {
            return timeline[0];
        }

        for (int i = 0; i < timeline.Length - 1; i++)
        {
            if (timeline[i].time <= alpha && timeline[i + 1].time > alpha)
            {
                float a = (alpha - timeline[i].time) / (timeline[i + 1].time - timeline[i].time);
                float lerp = Mathf.InverseLerp(timeline[i].time, timeline[i + 1].time, a);
                color = Color.Lerp(timeline[i].color, timeline[i + 1].color, lerp);

                int what = alpha >= (timeline[i].time + timeline[i + 1].time / 2f) ? i + 1 : i;
                
                return new Point(timeline[what].time,timeline[what].biomeType, color,timeline[what].resourceType,timeline[what].resources);
            }
        }

        return timeline[timeline.Length - 1];
    }
}
