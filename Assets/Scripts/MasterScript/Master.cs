using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the master is devided into three scripts: the Enviroment, the Factions and the Interface, these will interact with each other. 
[RequireComponent(typeof(Enviroment))]
[RequireComponent(typeof(Factions))]
[RequireComponent(typeof(Interface))]

//Variety makes different building types easily accessible to all scripts. This process is being depreicated to allow empires to have their own unique set of ships and structures. 
[RequireComponent(typeof(Variety))]
public class Master : MonoBehaviour
{
    public float averageFrameRate = 60f;
    public float frameRate = 1f;
    public float lowestFrameRate = 60f;
    private float seconds = 0.0f;

    public static Master instance;
    public Variety variety;
    public Enviroment enviroment;
    public Factions factions;
    public Interface userInterface;

    public int seed = 0;

    private PathFindAlgorithm pathFindAlgorithm;

    // Start is called before the first frame update
    void Awake()
    {
        Random.InitState(seed);
        if (Singleton())
        {
            //enviroment = GetComponent<Enviroment>();
            //factions = GetComponent<Factions>();
            // userInterface = GetComponent<Interface>();



        }
        pathFindAlgorithm = new PathFindAlgorithm();
        enviroment = GetComponent<Enviroment>();
        factions = GetComponent<Factions>();
        userInterface = GetComponent<Interface>();
        variety = GetComponent<Variety>();

        variety.Initialise();

        factions.SpawnFactions(variety.builtShips,variety.builtStructures);

        enviroment.RandomGrid(5);

        //set the main camera to be above the players home world.
        Vector3 position = factions.factions[0].territory[0].transform.position;
        Camera.main.transform.position = new Vector3(position.x, position.y, -10);
        /*
        //function to test ship movement, all the ships will move to the first star ever created.
        int targetPosition = enviroment.stars[0].position;
       
        for (int i = 1; i < enviroment.stars.Length; i++)
        {
            enviroment.stars[i].starShipManager.fleets[0].SetPath(PathFind(enviroment.stars[i].position, enviroment.stars[0].position));
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        frameRate = 1f / Time.deltaTime;
        if (frameRate < lowestFrameRate && seconds > 3f)
        {
            lowestFrameRate = frameRate;
        }

        if (seconds < 3f)
        {
            averageFrameRate = frameRate;
        }
        else
        {
            averageFrameRate = (averageFrameRate + frameRate) / 2f;
        }
    }

    bool Singleton()
    {
        //singleton. There is always one master script in the level.
        if (instance == null)
        {
            instance = this;
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }


    //used to convert between two dimensional arrays(filled one collumm at a time) to one dimensional arrays. 
    public int TwoDimToOneDim(int x, int y, int height)
    {
        return x * height + y;

    }

    public void OneDimToTwoDim(int xy, int height, out int x, out int y)
    {
        float value = (float)xy / (float)height;

        y = xy % height;
        x = Mathf.FloorToInt(value);
    }



    public class Node {
        public int position;
        public int lastPosition;
        public float distance;

        public Node(int position, int lastPosition, float distance)
        {
            this.position = position;
            this.lastPosition = lastPosition;
            this.distance = distance;
        }

    }

    //Pathfind algorithm could be used in multiple different scripts: 

    public List<int> PathFind(int start, int end, int[] factionsAllowed = null, float maxLength = 0f)
    {
        /*
        List<Master.Node> frontier = new List<Master.Node>();
        List<Master.Node> visited = new List<Master.Node>();


        Master.Node current = new Master.Node(start, -1, 0f);

        frontier.Add(current);
        visited.Add(current);

        //Master.instance.enviroment.GetNeighbouringTiles(start);

        //int iterations = 100;
        while (frontier.Count > 0)
        {
            frontier.Remove(current);

            if (current.position == end)
            {
                break;
            }
            else
            {
                Expand(current, frontier, visited,end);
            }

            if (frontier.Count > 0)
            {
                current = SmallestNode(frontier);
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

       return pathFindAlgorithm.PathFind(start, end,factionsAllowed,maxLength);
    }
    /*
        return ReadPath(start,current,visited);
    */

   




    public Master.Node SmallestNode(List<Master.Node> frontier)
    {
        float smallest = 0f;
        int smallestIndex = 0;

        for (int i = 0; i < frontier.Count; i++)
        {
            if (frontier[i].distance < smallest || i == 0)
            {
                smallest = frontier[i].distance;
                smallestIndex = i;
            }
        }

        return frontier[smallestIndex];
    }
    void Expand(Master.Node node, List<Master.Node> frontier, List<Master.Node> visited, int end)
    {
        List<int> neighbours = Master.instance.enviroment.grid[node.position].starConnections.connections;

       // int counter = 0;
        for (int n = 0; n < neighbours.Count; n++)
        {
            bool found = false;
            for (int v = 0; v < visited.Count; v++)
            {
                if (visited[v].position == neighbours[n])
                {
                    //print("neighbour already found: " + neighbours[n]);
                    //counter++;
                    found = true;
                    float distanceToEnd = DistanceBetweenTwoPoints(neighbours[n], end);
                    Master.Node neighbour = new Master.Node(neighbours[n], node.position, node.distance + 1f + distanceToEnd);
                    if (visited[v].distance > neighbour.distance)
                    {
                        
                        visited.RemoveAt(v);
                        visited.Insert(v,neighbour);
                        frontier.Add(neighbour);
                    }
                    break;
                    
                }

            }
            if (!found)
            {
                float distanceToEnd = DistanceBetweenTwoPoints(neighbours[n], end);
                Master.Node neighbour = new Master.Node(neighbours[n], node.position, node.distance + 1f + distanceToEnd);
                visited.Add(neighbour);
                frontier.Add(neighbour);
            }
        }

        //print("neighbours visited before = " + counter);

    }

    float DistanceBetweenTwoPoints(int a, int b)
    {
        OneDimToTwoDim(a, enviroment.rows, out int aX, out int aY);
        OneDimToTwoDim(b, enviroment.rows, out int bX, out int bY);

        return Vector2.Distance(new Vector2(aX,aY), new Vector2(bX,bY));
    }

    //reads the path created by the pathfinding algorithm, by following the breadcrumbs backwards.
    List<int> ReadPath(int startPosition, Node endNode, List<Node> visited)
    {
        List<int> path = new List<int>();
        path.Add(endNode.position);
       

        bool found = false;
        Node current = endNode;
        while (!found)
        {
            bool crash = true;
            for(int i = 0; i < visited.Count; i++)
            {
                if(current.position == startPosition)
                {
                    found = true;
                    crash = false;
                    break;
                }


                if(visited[i].position == current.lastPosition)
                {
                    path.Add(visited[i].position);
                    current = visited[i];
                    crash = false;
                    break;
                }
            }
            if (crash)
            {
                print("Prevent crash");
                return new List<int>();
            }
        }

        path.Reverse();
        return path;
    }

    
   
}
