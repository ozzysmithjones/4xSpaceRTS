using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate double StarEvalFunc(Star star);

public class PathFindAlgorithm
{
    private Star[] stars;
    private static ulong iteration = 0;

    public PathFindAlgorithm()
    {
        Enviroment enviroment = Master.instance.enviroment;
        stars = enviroment.stars;
    }

    private static int SelectLowest(List<Star> stars)
    {
        double lowestf = int.MaxValue;
        int  index = 0;

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

    private static double Heuristic(Star a, Star b)
    {
        int xDiff = b.x - a.x;
        int yDiff = b.y - a.y;
        return Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) * 11;
    }

    private static double Step(Star a, Star b)
    {
        int xDiff = Mathf.Abs(b.x - a.x);
        int yDiff = Mathf.Abs(b.y - a.y);
        return (xDiff * 10) + (yDiff * 10) - (xDiff * yDiff * 6); //10 for orthogonal, 14 for diagonal.
    }


    public List<Star> FindPath(Star start, Star end)
    {
        if(start == end)
        {
            return new List<Star>();
        }

        ++iteration; //increment iteration instead of setting all nodes to "not in open", performance improvement.
        if(iteration == ulong.MaxValue) //reset all iterations when wrapping to prevent any possible bugs.
        {
            foreach(Star star in stars)
            {
                star.node.iteration = 0;
            }

            iteration = 1;
        }

        end.node.breadcrumb = null;
        end.node.g = 0;
        end.node.f = Heuristic(end, start);
        end.node.iteration = iteration;
        end.node.inOpen = true;

        List<Star> open = new List<Star>() { end };
        Star current = null;

        while (open.Count > 0)
        {
            {
                int index = SelectLowest(open);
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
            double g;

            foreach (Star neighbour in connections)
            {
                g = current.node.g + Step(current, neighbour);

                if(neighbour.node.iteration != iteration)
                {
                    neighbour.node.iteration = iteration;
                    neighbour.node.breadcrumb = current;
                    neighbour.node.g = g;
                    neighbour.node.f = neighbour.node.g + Heuristic(neighbour, start);
                    neighbour.node.inOpen = true;
                    open.Add(neighbour);
                }
                else if(g < neighbour.node.g)
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

        ++iteration; //increment iteration instead of setting all nodes to "not in open", performance improvement.
        if (iteration == ulong.MaxValue) //reset all iterations when wrapping to prevent any possible bugs.
        {
            foreach (Star star in stars)
            {
                star.node.iteration = 0;
            }

            iteration = 1;
        }

        start.node.iteration = iteration;
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
            double g = current.node.g + 1;

            foreach(Star neighbour in connections)
            {
                if(neighbour.node.iteration != iteration)
                {
                    neighbour.node.iteration = iteration;
                    neighbour.node.g = g;
                    open.Enqueue(neighbour);
                    presence.Add(neighbour);
                }
            }
        }

        return presence;
    }
    
    public List<Star> FindBestPath(Star start, int maxDepth, StarEvalFunc eval)
    {
        ++iteration; //increment iteration instead of setting all nodes to "not in open", performance improvement.
        if (iteration == ulong.MaxValue) //reset all iterations when wrapping to prevent any possible bugs.
        {
            foreach (Star star in stars)
            {
                star.node.iteration = 0;
            }

            iteration = 1;
        }

        start.node.breadcrumb = null;
        start.node.g = 0;
        start.node.f = eval(start);
        start.node.iteration = iteration;
        start.node.inOpen = true;

        Queue<Star> open = new Queue<Star>();
        open.Enqueue(start);
        Star best = start;

        while (open.Count > 0)
        {
            Star current = open.Dequeue();
            current.node.inOpen = false;

            if(current.node.f > best.node.f)
            {
                best = current;
            }

            if (current.node.g >= maxDepth)
            {
                continue;
            }

            List<Star> connections = current.starConnections.connections;
            double g = current.node.g + 1;

            foreach (Star neighbour in connections)
            {
                double score = eval(neighbour) + current.node.f;

                if (neighbour.node.iteration != iteration)
                {
                    neighbour.node.iteration = iteration;
                    neighbour.node.breadcrumb = current;
                    neighbour.node.g = g;
                    neighbour.node.f = score;
                    neighbour.node.inOpen = true;
                    open.Enqueue(neighbour);
                }
                else if(neighbour.node.f < score && neighbour.node.g > current.node.g)
                {
                    neighbour.node.iteration = iteration;
                    neighbour.node.breadcrumb = current;
                    neighbour.node.g = g;
                    neighbour.node.f = score;

                    if(!neighbour.node.inOpen)
                    {
                        neighbour.node.inOpen = true;
                        open.Enqueue(neighbour);
                    }
                }
            }
        }

        List<Star> path = new List<Star>();
        while (best != null)
        {
            path.Add(best);
            best = best.node.breadcrumb;
        }

        path.Reverse();

        return path;
    }
        
}

