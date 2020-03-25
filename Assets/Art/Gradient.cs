//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


public class Gradient : MonoBehaviour
{
    [SerializeField]
    private Point[] timeline;



    [System.Serializable]
    public class Point
    {
        public float time = 0.0f;
        public Color color;

        public Point(float t, Color c)
        {
            time = t;
            color = c;
        }

    }


    public Gradient(Point[] points)
    {

        timeline = new Gradient.Point[points.Length];
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
                float a = (alpha - timeline[i].time) / (timeline[i + 1].time - timeline[i].time);
                float lerp = Mathf.InverseLerp(timeline[i].time, timeline[i + 1].time, a);
                color = Color.Lerp(timeline[i].color, timeline[i + 1].color, lerp);
                return color;
            }
        }

        return timeline[timeline.Length - 1].color;
    }

    //public Gradient(){

    //}
}
