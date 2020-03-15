using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSearchAlgorithm
{
    public static GraphSearchAlgorithm graphSearchAlgorithm;
    public delegate bool ShouldEnd(Node node);
    public delegate bool ShouldExpand(Node node, List<Node> visited);
    public delegate float NodeValue(Node node);


    public delegate bool ShouldPathEnd(PathNode pathNode);
    public delegate bool ShouldPathExpand(PathNode pathNode, List<PathNode> visited);
    public delegate float PathNodeValue(PathNode pathNode);


    private Star[] stars;

    public void Initialise()
    {
        graphSearchAlgorithm = this;
        stars = Master.instance.enviroment.stars;
    }


    public struct Node
    {
        public int coordinate;
        public int lastCoordinate;

        public float value;

        public Node(int coordinate, int lastCoordinate, float value)
        {
            this.coordinate = coordinate;
            this.lastCoordinate = lastCoordinate;
            this.value = value;
        }

    }

    public struct PathNode
    {
        public int coordinate;
        public int lastCoordinate;

        public float value;
        public float distanceToBegining;
        public float distanceToEnd;

        public PathNode(int coordinate, int lastCoordinate, float value, float distanceToBegining, float distanceToEnd)
        {
            this.coordinate = coordinate;
            this.lastCoordinate = lastCoordinate;
            this.value = value;
            this.distanceToBegining = distanceToBegining;
            this.distanceToEnd = distanceToEnd;
        }
    }


    List<int> NeighboursOf(int coordinate)
    {
        return stars[coordinate].starConnections.connections;
    }

    //allows a custom search algorithm to be developed outside of the class, with it's own functions as inputs. There may be a planning sreach algorithm  or a economic/militaristic breadth first ect.
    public Node Search(int startCoordinate,NodeValue nodeValue,ShouldExpand shouldExpand, ShouldEnd shouldEnd)
    {
        List<Node> frontier = new List<Node>();
        List<Node> visited = new List<Node>();

        Node first = new Node(startCoordinate, -1, 0.0f);
        first.value = nodeValue(first);

        frontier.Add(first);
        visited.Add(first);

        Node current = frontier[0];
        while (!shouldEnd(current))
        {
            frontier.RemoveAt(0);
            Expand(current,nodeValue,shouldExpand,visited,frontier);
            if (frontier.Count <= 0)
            {
                break;
            }
            current = frontier[0];
        }

        return current;

    }

    void Expand(Node node, NodeValue nodeValue, ShouldExpand shouldExpand, List<Node> visited, List<Node> frontier)
    {
        List<int> neighbours = NeighboursOf(node.coordinate);

        for(int i = 0; i < neighbours.Count; i++)
        {

            Node neighbourNode = new Node(neighbours[i], node.coordinate, 0.0f);
            neighbourNode.value = nodeValue(neighbourNode);
            if (shouldExpand(neighbourNode, visited))
            {
                visited.Add(neighbourNode);
                InsertIntoFrontierByValue(neighbourNode,frontier);
            }
        }

    }

    void ExpandPath(PathNode node, PathNodeValue nodeValue, ShouldPathExpand shouldExpand, List<PathNode> visited, List<PathNode> frontier, int endCoordinate)
    {
        List<int> neighbours = NeighboursOf(node.coordinate);

        for (int i = 0; i < neighbours.Count; i++)
        {

            PathNode neighbourNode = new PathNode(neighbours[i], node.coordinate, 0.0f,Distance(node.coordinate,neighbours[i]),Distance(neighbours[i],endCoordinate));
            neighbourNode.value = nodeValue(neighbourNode);
            if (shouldExpand(neighbourNode, visited))
            {
                visited.Add(neighbourNode);
                InsertIntoFrontierByValue(neighbourNode, frontier);
            }
        }

    }




    void InsertIntoFrontierByValue(Node node, List<Node> frontier)
    {
        for(int i = 0; i < frontier.Count; i++)
        {
            if(frontier[i].value >= node.value)
            {
                frontier.Insert(i, node);
                return;
            }
        }
        frontier.Add(node);
    }

    void InsertIntoFrontierByValue(PathNode node, List<PathNode> frontier)
    {
        for (int i = 0; i < frontier.Count; i++)
        {
            if (frontier[i].value >= node.value)
            {
                frontier.Insert(i, node);
                return;
            }

        }
        frontier.Add(node);
    }



    //pathfind algorithm, could be quite common, so it's made in here. 
    public List<int> PathFind(int startCoordinate, int endCoordinate, PathNodeValue nodeValue, ShouldPathExpand shouldExpand)
    {
        List<PathNode> frontier = new List<PathNode>();
        List<PathNode> visited = new List<PathNode>();

        PathNode first = new PathNode(startCoordinate, -1, 0.0f,0.0f,Distance(startCoordinate,endCoordinate));
        first.value = nodeValue(first);

        frontier.Add(first);
        visited.Add(first);

        PathNode current = frontier[0];
        while (current.coordinate != endCoordinate)
        {
            frontier.RemoveAt(0);
            ExpandPath(current, nodeValue, shouldExpand, visited, frontier, endCoordinate);
            if (frontier.Count <= 0)
            {
                break;
            }
            current = frontier[0];
        }
        return ReadPath(current,startCoordinate,visited);
    }

    float Distance(int a, int b)
    {
        Calculation.OneDimToTwoDim(a, out int aX, out int aY);
        Calculation.OneDimToTwoDim(b, out int bX, out int bY);

        return Vector2.Distance(new Vector2(aX, aY), new Vector2(bX, bY));
    }


    List<int> ReadPath(PathNode node, int startCoordinate, List<PathNode> visited)
    {
        List<int> path = new List<int>();
        path.Add(node.coordinate);


        bool found = false;
        PathNode current = node;
        while (!found)
        {
            bool crash = true;
            for (int i = 0; i < visited.Count; i++)
            {
                if (current.coordinate == startCoordinate)
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
                Debug.LogError("Prevented crash");
                return new List<int>();
            }
        }

        path.Reverse();
        return path;
    }

   
    //A star pathfind algorithm. 

    public List<int> AStarPathFind(int startCoordinate, int endCoordinate)
    {
        return PathFind(startCoordinate, endCoordinate, AStarPathNodeValue, AStarShouldExpand);
    }


    float AStarPathNodeValue(PathNode pathNode)
    {
        return pathNode.distanceToBegining + pathNode.distanceToEnd;
    }

    bool AStarShouldExpand(PathNode pathNode, List<PathNode> visited)
    {
        for (int i = 0; i < visited.Count; i++)
        {
            if (visited[i].coordinate == pathNode.coordinate)
            {
                if (visited[i].value > pathNode.value)
                {
                    visited.RemoveAt(i);
                    i--;
                    continue;
                }
                return false;
            }
        }

        return true;
    }

    //end A* pathfind algorithm


}
