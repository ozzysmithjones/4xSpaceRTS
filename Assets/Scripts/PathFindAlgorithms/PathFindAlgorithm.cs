using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    public bool closed;
    public int coordinate,lastCoordinate;
    public int distanceToBegining, distanceToEnd;

    public Node(int coordinate,int lastCoordinate, int distanceToBegining, int distanceToEnd)
    {
        this.coordinate = coordinate;
        this.lastCoordinate = lastCoordinate;
        this.distanceToBegining = distanceToBegining;
        this.distanceToEnd = distanceToEnd;
        this.closed = false;
    }
}

public class PathFindAlgorithm
{
    public static PathFindAlgorithm instance;

    private Star[] stars;
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();

    private int empire;
    private bool friendlyOnly;

    public PathFindAlgorithm()
    {
        stars = Master.instance.enviroment.stars;
        instance = this;
    }

    public List<int> Path(int start, int end,int empire = -1,bool friendlyOnly = false)
    {
        this.empire = empire;
        this.friendlyOnly = friendlyOnly;

        Node current = new Node(start, -1, 0, (int)Calculation.SquareDistance(start, end));
        open.Add(current);

        while (open.Count > 0 && current.coordinate != end)
        {
            int currentIndex = GetCurrentNodeIndex();
            current = open[currentIndex];
            open.RemoveAt(currentIndex);
            closed.Add(current);
            Search(current, end);

        }
        List<int> path = ReadPath(current);
        open.Clear();
        closed.Clear();
        return path;
    }

    private void Search(Node node, int endCoordinate)
    {
        List<Star> neighbours = stars[node.coordinate].starConnections.GetConnectedStars();

        for(int i = 0; i < neighbours.Count; i++)
        {
            Node neighbourNode = new Node(neighbours[i].index, node.coordinate, node.distanceToBegining + (int)Calculation.SquareDistance(node.coordinate, neighbours[i].index), (int)Calculation.SquareDistance(neighbours[i].index, endCoordinate));
            CheckVisit(neighbourNode);
        }
    }

    private bool CheckVisit(Node node)
    {
        for(int i = 0; i < closed.Count; i++)
        {
            if(closed[i].coordinate == node.coordinate)
            {
                return false;
            }
        }
        for(int i = 0; i < open.Count; i++)
        {
            if(open[i].coordinate == node.coordinate)
            {
                if(open[i].distanceToBegining > node.distanceToBegining)
                {
                    open[i] = node;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        if (friendlyOnly)
        {
            if(stars[node.coordinate].factionIndex != empire)
            {
                closed.Add(node);
                return false;
            }
        }
        open.Add(node);
        return true;
    }

    private int GetCurrentNodeIndex()
    {
        int smallest = 0;
        int index = 0;
        for(int i = 0; i < open.Count; i++)
        {
            if(open[i].distanceToBegining + open[i].distanceToEnd < smallest || i == 0)
            {
                smallest = open[i].distanceToBegining + open[i].distanceToEnd;
                index = i;
            }
        }
        return index;
    }

    private List<int> ReadPath(Node endNode)
    {
        List<int> path = new List<int>();
        path.Add(endNode.coordinate);
        Node current = endNode;
        while (current.lastCoordinate >= 0)
        {
            bool crash = true;
            for(int i = 0; i < closed.Count; i++)
            {
                if(closed[i].coordinate == current.lastCoordinate)
                {
                    path.Add(closed[i].coordinate);
                    current = closed[i];
                    crash = false;

                    if (current.lastCoordinate < 0)
                        break;
                }
            }
            if (crash)
            {
                Debug.LogError("Pathfinding Crash");
                break;
            }
        }
        path.Reverse();
        return path;
    }

}

