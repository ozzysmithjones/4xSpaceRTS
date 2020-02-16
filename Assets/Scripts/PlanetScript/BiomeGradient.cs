using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGradient  : MonoBehaviour
{


    [SerializeField]
    private Point[] timeline;



    [System.Serializable]
    public class Point
    {
        public float time = 0.0f;
        public string name = "Barren";
        public Color color;

        //0 = nothing.
        //1 = energy.
        //2 = materials.
        //3 = death matter.
        public int resourceType = 0;
        public int resources = 0;
      

        

        public Point(float t,string n, Color c, int type, int amount)
        {
            name = n;
            time = t;
            color = c;

            resourceType = type;
            resources = amount;
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
                
                return new Point(timeline[what].time,timeline[what].name, color,timeline[what].resourceType,timeline[what].resources);
            }
        }

        return timeline[timeline.Length - 1];
    }
}
