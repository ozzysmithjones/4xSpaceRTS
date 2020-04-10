using System.Collections.Generic;
using UnityEngine;

/*
public class GraphNode
{
    public int coordinate;
    public int lastCoordinate;

    public float value;

    public GraphNode(int coordinate, int lastCoordinate, float value)
    {
        this.coordinate = coordinate;
        this.lastCoordinate = lastCoordinate;
        this.value = value;
    }

    public virtual float CalculateValue(GraphNode previousNode)
    {
        return value;
    }
    public virtual bool CanGoHere(List<GraphNode> visited, bool seekingSmallestNodeValue = true)
    {
        for (int i = 0; i < visited.Count; i++)
        {
            if (visited[i].coordinate == coordinate)
            {
                if (seekingSmallestNodeValue)
                {
                    if (visited[i].value > value)
                    {
                        visited.RemoveAt(i);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (visited[i].value < value)
                    {
                        visited.RemoveAt(i);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public virtual bool EndHere()
    {
        return false;
    }

    public virtual void Expand(List<GraphNode> frontier, List<GraphNode> visited, bool seekingSmallestNodeValue = true)
    {
        List<int> neighbours = Master.instance.enviroment.stars[coordinate].starConnections.connections;

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i] == lastCoordinate)
            {
                continue;
            }

            GraphNode neighbourNode = CreateNeighbour(neighbours[i]);
            neighbourNode.CalculateValue(this);


            if (neighbourNode.CanGoHere(visited, seekingSmallestNodeValue))
            {

                visited.Add(neighbourNode);
                frontier.Add(neighbourNode);
            }

        }
    }

    protected virtual GraphNode CreateNeighbour(int neighbourCoordinate)
    {

        return new GraphNode(neighbourCoordinate, coordinate, 0.0f);
    }

}

public class GraphSearchAlgorithms
{
    public static GraphSearchAlgorithms instance;

    public void Initialise()
    {
        instance = this;
    }

    public GraphNode Search(GraphNode first, List<GraphNode> visited, bool seekingSmallestNodeValue = true)
    {
        List<GraphNode> frontier = new List<GraphNode>();
        GraphNode current = first;

        frontier.Add(first);
        visited.Add(first);
        int removeIndex = 0;
        int iterations = 0;

        while (frontier.Count > 0 && !current.EndHere())
        {
            frontier.RemoveAt(removeIndex);
            current.Expand(frontier, visited);
            if (frontier.Count <= 0)
            {
                break;
            }
            removeIndex = GetNodeIndexByValue(frontier, seekingSmallestNodeValue);
            current = frontier[removeIndex];
        }
        return current;

    }

    public List<int> PathFind(int start, int end)
    {
        if (start == end)
        {
            return new List<int>();
        }
        PathNode first = new PathNode(start, -1, Calculation.SquareDistance(start, end) * 10, end, 0, (int)(Calculation.SquareDistance(start, end) * 10));
        List<GraphNode> visited = new List<GraphNode>();
        GraphNode last = Search(first, visited);

        return ReadPath(start, last, visited);
    }


    int GetNodeIndexByValue(List<GraphNode> nodes, bool smallest = true)
    {

        float greatestValue = nodes[0].value;
        int index = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].value < greatestValue && smallest)
            {
                greatestValue = nodes[i].value;
                index = i;

            }
            else if (nodes[i].value > greatestValue && !smallest)
            {
                greatestValue = nodes[i].value;
                index = i;

            }


        }
        return index;

    }



    private List<int> ReadPath(int start, GraphNode endNode, List<GraphNode> visited)
    {

        List<int> path = new List<int>();
        path.Add(endNode.coordinate);

        bool found = false;
        GraphNode current = endNode;
        while (!found)
        {
            bool crash = true;
            for (int i = 0; i < visited.Count; i++)
            {
                if (current.coordinate == start)
                {
                    found = true;
                    crash = false;
                    break;
                }
                if (visited[i].coordinate == current.lastCoordinate)
                {
                    path.Add(visited[i].coordinate);
                    current = visited[i];
                    crash = false;
                    break;
                }
            }
            if (crash)
            {
                Debug.LogError("Prevented crash with pathfinding");
                for (int i = 0; i < path.Count; i++)
                {
                    Debug.LogError("path so far" + i + " is at coordinate " + path[i]);
                }
                return new List<int>();
            }
        }

        path.Reverse();
        return path;


    }




}
*/

