using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    //breadth first search for nearest starts.
    public void ConnectToNearestStars(int start, int minConnectedStars = 1)
    {
        List<Master.Node> frontier = new List<Master.Node>();
        List<Master.Node> visited = new List<Master.Node>();


        Master.Node current = new Master.Node(start, -1, 0f);

        frontier.Add(current);
        visited.Add(current);

        //Master.instance.enviroment.GetNeighbouringTiles(start);
        
        //int iterations = 100;
        while(frontier.Count > 0)
        {
            frontier.Remove(current);

            if (Check(current) && current.position != start)
            {
                connections.Add(current.position);
                minConnectedStars--;
                if (minConnectedStars <= 0)
                {
                    break;
                }
            }
            else
            {
                Expand(current,frontier,visited);
            }

            if (frontier.Count > 0)
            {
                current = Master.instance.SmallestNode(frontier);
            }

            /*
            //print("frontier size = " + frontier.Count + " visited size = " + visited.Count);
            iterations--;
            if(iterations <= 0)
            {
                print("prevent crash.");
                break;
            }
            */
        }
        
        Connect(start);

    }


    void Expand(Master.Node node,List<Master.Node> frontier,List<Master.Node> visited)
    {
        List<int> neighbours = Master.instance.enviroment.GetNeighbouringTiles(node.position);

        //int counter = 0; 
        for (int n = 0; n < neighbours.Count; n++)
        {
           bool found = false;
           for(int v = 0; v < visited.Count; v++)
            {
                if(visited[v].position == neighbours[n])
                {
                    
                    found = true;
                    break;
                    /*
                    if(visited[v].distance > node.distance + 1f)
                    {
                        Master.Node newNode = new Master.Node(neighbours[n], node.position, node.distance + 1f);
                        visited.RemoveAt(v);
                        visited.Insert(v,newNode);
                        frontier.Add(newNode);
                    }
                    */
                }
                
            }
            if (!found)
            {
                Master.Node neighbour = new Master.Node(neighbours[n], node.position, node.distance + 1f);
                visited.Add(neighbour);
                frontier.Add(neighbour);
            }
        }

       
       
    }

    bool Check(Master.Node node)
    {

        if(Master.instance.enviroment.stars[node.position] != null)
        {
            if (!connections.Contains(node.position))
            {
                return true;
            }
        }
        return false;
    }

    

    void Connect(int center)
    {
        //the other stars connect to this star.

        for(int i = 0; i < connections.Count; i++)

        {   //get the other stars connections.
            StarConnections starConnections;
            if (Master.instance.enviroment.stars[connections[i]].starConnections == null)
            {
                starConnections = Master.instance.enviroment.stars[connections[i]].GetComponent<StarConnections>();
                Master.instance.enviroment.stars[connections[i]].starConnections = starConnections;
            }
            else
            {
                starConnections = Master.instance.enviroment.stars[connections[i]].starConnections;
            }
            //connect that star to this one.
            if (!starConnections.connections.Contains(center))
            {
                starConnections.connections.Add(center);
                LineRenderer line = DrawLine(center, connections[i]);

                starConnections.lines.Add(line);
                lines.Add(line);
                linesStartingHere.Add(i);

                //create the gateway on this side and that side. 
                SpawnStarGate((Vector2)Master.instance.enviroment.stars[connections[i]].transform.position, connections[i]);
                starConnections.SpawnStarGate((Vector2)transform.position, center);

            }
        }

        
    }
    
    LineRenderer DrawLine(int start, int end, float starRadius = 15f)
    {
        //the two transforms to draw the line between.
        Star startStar = Master.instance.enviroment.stars[start];
        Star endStar = Master.instance.enviroment.stars[end];



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
        //fog of war purposes.
        //line.gameObject.SetActive(false);

        
        //line.enabled = false;
        

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
        if(star == null || other == null)
        {
            Debug.Log("objects are null");
        }
       

        if(other.starConnections == null)
        {
            Debug.Log("starConnections is null");
        }else if(other.starConnections.connections == null)
        {
            Debug.Log("array is null");
        }
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
