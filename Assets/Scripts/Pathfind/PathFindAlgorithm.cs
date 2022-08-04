using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFindAlgorithm
{
    private Star[] stars;

    public PathFindAlgorithm()
    {
        Enviroment enviroment = Master.instance.enviroment;
        stars = enviroment.stars;
    }

    private static int Select(List<Star> stars)
    {
        int lowestf = int.MaxValue;
        int index = 0;

        for(int i = 0; i < stars.Count; ++i)
        {
            if(stars[i].node.f < lowestf)
            {
                lowestf = stars[i].node.f;
                index = i;
            }
        }


        return index;
    }

    private static int Heuristic(Star a, Star b)
    {
        int xDiff = b.x - a.x;
        int yDiff = b.y - a.y;
        return (int)(Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) * 11);
    }

    private static int Step(Star a, Star b)
    {
        int xDiff = Mathf.Abs(b.x - a.x);
        int yDiff = Mathf.Abs(b.y - a.y);
        return (xDiff * 10) + (yDiff * 10) - (xDiff * yDiff * 6); //10 for orthogonal, 14 for diagonal.
    }


    public List<Star> Path(Star start, Star end)
    {
        if(start == end)
        {
            return new List<Star>();
        }

        foreach(Star star in stars)
        {
            star.node.inOpen = false;
            star.node.g = int.MaxValue;
            star.node.breadcrumb = null;
        }

        end.node.g = 0;
        end.node.f = Heuristic(end, start);
        end.node.inOpen = true;
        List<Star> open = new List<Star>() { end };
        Star current = null;

        while (open.Count > 0)
        {
            {
                int index = Select(open);
                current = open[index];
                current.node.inOpen = false;
                open[index] = open[open.Count - 1];
                open.RemoveAt(open.Count - 1);
            }

            if (current == start)
            {
                break;
            }

            List<Star> connections = current.starConnections.connections;
            int g;

            foreach (Star neighbour in connections)
            {
                g = current.node.g + Step(current, neighbour);

                if(g < neighbour.node.g)
                {
                    neighbour.node.breadcrumb = current;
                    neighbour.node.g = g;
                    neighbour.node.f = neighbour.node.g + Heuristic(neighbour, start);

                    if(!neighbour.node.inOpen)
                    {
                        neighbour.node.inOpen = true;
                        open.Add(neighbour);
                    }
                }
            }
        }


        List<Star> path = new List<Star>();

        if(current != start)
        {
            return path;
        }

        while(current != null)
        {
            path.Add(current);
            current = current.node.breadcrumb;
        }

        return path;
    }


    public List<Star> Presence(Star start, int depth)
    {
        start.node.g = 0;
        List<Star> presence = new List<Star>();
        presence.Add(start);

        foreach (Star star in stars)
        {
            star.node.inOpen = false;
        }

        Queue<Star> open = new Queue<Star>();
        open.Enqueue(start);

        while(open.Count > 0)
        {
            Star current = open.Dequeue();

            if(current.node.g >= depth)
            {
                continue;
            }

            List<Star> connections = current.starConnections.connections;
            int g = current.node.g + 1;

            foreach(Star neighbour in connections)
            {
                if(!neighbour.node.inOpen)
                {
                    neighbour.node.inOpen = true;
                    neighbour.node.g = g;
                    open.Enqueue(neighbour);
                    presence.Add(neighbour);
                }
            }
        }

        return presence;
    }
}

