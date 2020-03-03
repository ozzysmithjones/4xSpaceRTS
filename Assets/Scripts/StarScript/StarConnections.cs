using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StarGate
{
    public Transform gate;
    public int targetStarIndex;

    public StarGate(Transform form, int connection)
    {
        gate = form;
        targetStarIndex = connection;
    }
}

public class StarConnections : MonoBehaviour
{
    public GameObject gateParent;
    public GameObject linePrefab;
    public GameObject starGatePrefab;

    private List<StarGate> starGates = new List<StarGate>();

    //private List<LineRenderer> lines = new List<LineRenderer>();
    public List<int> connections = new List<int>();
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
        line.startColor = startStar.factionIndex >= 0 ? Master.instance.factions.factions[startStar.factionIndex].flagColor : line.startColor;
        line.endColor = endStar.factionIndex >= 0 ? Master.instance.factions.factions[endStar.factionIndex].flagColor : line.endColor;

        return line;
    }

    public void ChangeColor(Color color)
    {
        for(int i = 0; i < lines.Count; i++)
        {
            if(lines[i] == null)
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
        List<Star> stars = new List<Star>();
        for(int i = 0; i < connections.Count; i++)
        {
            Star star = Master.instance.enviroment.stars[connections[i]];
            if(star != null)
            {
                stars.Add(star);
            }
        }

        return stars;
    }

    public int GetConnectionToStar(int star)
    {
        for(int i = 0; i < connections.Count; i++)
        {
            if(connections[i] == star)
            {
                return i;
            }
        }
        Debug.LogError("couldn't find what you were looking for");
        return connections[0];
    }
    void SpawnStarGate(Vector2 targetStar,int targetStarPosition, float distance = 15f)
    {
        Vector2 spawnPoint = Vector2.MoveTowards((Vector2)transform.position, targetStar, distance);
        Transform form = Instantiate(starGatePrefab, spawnPoint, Quaternion.identity).transform;

        StarGate starGate = new StarGate(form, targetStarPosition);

        starGate.gate.SetParent(gateParent.transform);

        starGates.Add(starGate);
        
    }

    public Transform GetStarGate(int targetStarIndex)
    {
        for (int i = 0; i < starGates.Count; i++){

           if(starGates[i].targetStarIndex == targetStarIndex)
            {
                return starGates[i].gate;
            }
        }
        Debug.LogError("Request denied for star gate service");
        return null;
    }


    public bool IsConnectedToFaction(int faction = -1)
    {
        List<Star> stars = GetConnectedStars();

        for(int i = 0; i < stars.Count; i++)
        {
            if(stars[i].factionIndex == faction)
            {
                return true;
            }
        }

        return false;
    }


    public static void Connect(Star star, Star other)
    {
        //connect that star to this one.
        if (!other.starConnections.connections.Contains(star.index))
        {

            star.starConnections.connections.Add(other.index);
            other.starConnections.connections.Add(star.index);

            LineRenderer line = star.starConnections.DrawLine(star.index,other.index);

            other.starConnections.lines.Add(line);
            star.starConnections.lines.Add(line);
            star.starConnections.linesStartingHere.Add(star.starConnections.connections.Count-1);

            //create the gateway on this side and that side. 
            star.starConnections.SpawnStarGate(other.transform.position, other.index);
            other.starConnections.SpawnStarGate(star.transform.position, star.index);

        }
    }



}
