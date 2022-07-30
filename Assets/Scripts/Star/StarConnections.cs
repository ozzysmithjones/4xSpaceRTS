using System.Collections.Generic;
using UnityEngine;

class StarGate
{
    public Transform gate;
    public Star targetStar;

    public StarGate(Transform form, Star targetStar)
    {
        gate = form;
        this.targetStar = targetStar;
    }
}

public class StarConnections : MonoBehaviour
{
    public Color normalLineColor = Color.grey;
    public GameObject gateParent;
    public GameObject linePrefab;
    public GameObject starGatePrefab;

    private List<StarGate> starGates = new List<StarGate>();

    //private List<LineRenderer> lines = new List<LineRenderer>();
    public List<Star> connections = new List<Star>();
    public List<LineRenderer> lines = new List<LineRenderer>();
    private List<int> linesStartingHere = new List<int>();



    LineRenderer DrawLine(int startCoordinates, int endCoordinates, float starRadius = 15f)
    {
        //the two transforms to draw the line between.
        Star startStar = Master.instance.enviroment.stars[startCoordinates];
        Star endStar = Master.instance.enviroment.stars[endCoordinates];

        //spawn a line object. 
        LineRenderer line = Instantiate(linePrefab, startStar.transform.position, startStar.transform.rotation).GetComponent<LineRenderer>();
        line.transform.SetParent(startStar.transform);

        Vector3 a = (Vector3)Vector2.MoveTowards((Vector2)startStar.transform.position, (Vector2)endStar.transform.position, starRadius);
        Vector3 b = (Vector3)Vector2.MoveTowards((Vector2)endStar.transform.position, (Vector2)startStar.transform.position, starRadius);

        //draw a line.
        line.SetPosition(0, a);
        line.SetPosition(1, b);

        //set correct colors:
        line.startColor = startStar.empire != null ? startStar.empire.flagColor : line.startColor * normalLineColor;
        line.endColor = endStar.empire != null ? endStar.empire.flagColor : line.endColor * normalLineColor;

        return line;
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i] == null)
            {
                return;
            }
            if (linesStartingHere.Contains(i))
            {
                lines[i].startColor = color;
            }
            else
            {
                lines[i].endColor = color;
            }



        }
    }

    public List<Star> GetConnectedStars()
    {
        return connections;
    }

    public int GetConnectionToStar(Star star)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i] == star)
            {
                return i;
            }
        }
        Debug.LogError("couldn't find what you were looking for");
        return 0;
    }


    void SpawnStarGate(Vector2 targetPosition, Star targetStar, float distance = 15f)
    {
        Vector2 spawnPoint = Vector2.MoveTowards((Vector2)transform.position, targetPosition, distance);
        Transform form = Instantiate(starGatePrefab, spawnPoint, Quaternion.identity).transform;

        StarGate starGate = new StarGate(form, targetStar);

        starGate.gate.SetParent(gateParent.transform);

        starGates.Add(starGate);

    }

    public Transform GetStarGate(Star targetStar)
    {
        for (int i = 0; i < starGates.Count; i++)
        {

            if (starGates[i].targetStar == targetStar)
            {
                return starGates[i].gate;
            }
        }
        Debug.LogError("Request denied for star gate service");
        return null;
    }


    public bool IsConnectedToEmpire(Empire empire)
    {
        List<Star> stars = GetConnectedStars();

        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i].empire == empire)
            {
                return true;
            }
        }

        return false;
    }


    public static void Connect(Star star, Star other)
    {
        //connect that star to this one.
        if (!other.starConnections.connections.Contains(star))
        {

            star.starConnections.connections.Add(other);
            other.starConnections.connections.Add(star);

            LineRenderer line = star.starConnections.DrawLine(star.index, other.index);

            other.starConnections.lines.Add(line);
            star.starConnections.lines.Add(line);
            star.starConnections.linesStartingHere.Add(star.starConnections.connections.Count - 1);

            //create the gateway on this side and that side. 
            star.starConnections.SpawnStarGate(other.transform.position, other);
            other.starConnections.SpawnStarGate(star.transform.position, star);

        }
    }



}
