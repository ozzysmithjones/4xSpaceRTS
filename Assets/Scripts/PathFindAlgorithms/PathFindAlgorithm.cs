using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindAlgorithm 
{

    int begining = 0;
    int ending = 0;

    private int[] factions;
    private float maxDistance = 0f;

    public class Node
    {
        public int position;
        public int lastPosition;
        public float distanceToBegining;
        public float distanceToEnd;

        public Node(int position, int lastPosition, float distanceToBegining, float distanceToEnd)
        {
            this.position = position;
            this.lastPosition = lastPosition;
            this.distanceToBegining = distanceToBegining;
            this.distanceToEnd = distanceToEnd;
        }

    }

    //Pathfind algorithm could be used in multiple different scripts: 

    public List<int> PathFind(int start, int end, int[] factionsAllowed = null, float maxDistance = 0f)
    {
        this.maxDistance = maxDistance;
        begining = start;
        ending = end;

        if (factionsAllowed != null)
        {
            factions = new int[factionsAllowed.Length];

            for (int i = 0; i < factionsAllowed.Length; i++)
            {
                factions[i] = factionsAllowed[i];
            }
        }
        else
        {
            factions = new int[0];
        }

        List<Node> frontier = new List<Node>();
        List<Node> visited = new List<Node>();

        float distanceToEnd = DistanceBetweenTwoPoints(start, end);
        Node current = new Node(start, -1, 0f,distanceToEnd);

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
                Expand(current, frontier, visited, end);
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
        }
        return ReadPath(start, current, visited);
    }

    public Node SmallestNode(List<Node> frontier)
    {
        float smallest = 0f;
        int smallestIndex = 0;

        for (int i = 0; i < frontier.Count; i++)
        {
            if (frontier[i].distanceToBegining + frontier[i].distanceToEnd < smallest || i == 0)
            {
                smallest = frontier[i].distanceToBegining + frontier[i].distanceToEnd;
                smallestIndex = i;
            }
        }

        return frontier[smallestIndex];
    }
    void Expand(Node node, List<Node> frontier, List<Node> visited, int end)
    {
        List<int> neighbours = Master.instance.enviroment.stars[node.position].starConnections.connections;

        // int counter = 0;
        for (int n = 0; n < neighbours.Count; n++)
        {
            if (Filter(neighbours[n]))
            {
                continue;
            }

            float distanceToEnd = DistanceBetweenTwoPoints(neighbours[n], end);
            float distanceToCenter = DistanceBetweenTwoPoints(neighbours[n], node.position);

            if  (node.distanceToBegining + Step(neighbours[n]) * distanceToCenter + distanceToEnd > maxDistance && maxDistance != 0f)
            {
                continue;
            }

            Node neighbour = new Node(neighbours[n], node.position, node.distanceToBegining + (Step(neighbours[n]) * distanceToCenter), distanceToEnd);


            bool found = false;
            for (int v = 0; v < visited.Count; v++)
            {
                if (visited[v].position == neighbours[n])
                {
                    //print("neighbour already found: " + neighbours[n]);
                    //counter++;
                    found = true;
                    
                    if (visited[v].distanceToBegining + visited[v].distanceToEnd > neighbour.distanceToBegining + neighbour.distanceToEnd)
                    {

                        visited.RemoveAt(v);
                        visited.Insert(v, neighbour);
                        frontier.Add(neighbour);
                    }
                    break;

                }

            }
            if (!found)
            {
               
                visited.Add(neighbour);
                frontier.Add(neighbour);
            }
        }

        //print("neighbours visited before = " + counter);

    }

    float DistanceBetweenTwoPoints(int a, int b)
    {
        OneDimToTwoDim(a, Master.instance.enviroment.rows, out int aX, out int aY);
        OneDimToTwoDim(b, Master.instance.enviroment.rows, out int bX, out int bY);

        return Vector2.Distance(new Vector2(aX, aY), new Vector2(bX, bY));
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
            for (int i = 0; i < visited.Count; i++)
            {
                if (current.position == startPosition)
                {
                    found = true;
                    crash = false;
                    break;
                }


                if (visited[i].position == current.lastPosition)
                {
                    path.Add(visited[i].position);
                    current = visited[i];
                    crash = false;
                    break;
                }
            }
            if (crash)
            {
                Debug.LogError("Prevented crash");
                return new List<int>();
            }
        }

        path.Reverse();
        return path;
    }

    public virtual bool Filter(int position)
    {
        if(position == begining || position == ending)
        {
            return false;
        }

        Star star = Master.instance.enviroment.stars[position];

        if(star == null)
        {
            
            return true;

        }else if (ArrayContains(factions, star.factionIndex) || factions.Length <= 0){

            return false;
        }
        else
        {
            return true;
        }

    }

    //this could be modified to make certain paths "longer", like traveling through mud or enemy territory.
    public virtual float Step(int position)
    {
        return 1f;
    }

    bool ArrayContains(int[] array, int item)
    {
        if (array.Length <= 0)
        {
            return false;
        }

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == item)
            {
                return true;
            }
        }
        return false;
    }

}
