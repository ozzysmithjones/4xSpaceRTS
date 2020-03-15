using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{

    public GameObject starPrefab;

    //public int Collums = 100;
    //public int Rows = 100;


    public float radius = 50f;
    private Master master;

    [Header("BASE VARIABLES")]
    private float space = 200f;
    public int numberOfStars = 100;
    public int collums = 40;
    public int rows = 40;

    private Star[] grid;
    public Star[] stars;


    public void BreadthFirstGenerate(int collums, int rows)
    {
        this.collums = collums;
        this.rows = rows;

        stars = new Star[numberOfStars];
        grid = new Star[collums * rows];

        List<Node> frontier = new List<Node>();
        int spawnedStars = 0;

        //spawn a star at the center.
        Star center = SpawnStar(collums / 2, rows / 2);
        center.index = 0;
        stars[0] = center;
        grid[TwoDimToOneDim(collums / 2, rows / 2, rows)] = center;
        spawnedStars++;

        //add it to the frontier and visited arrays.
        Node centerNode = new Node(TwoDimToOneDim(collums/2,rows/2,rows),-1,0);
        frontier.Add(centerNode);
       

        while (spawnedStars < numberOfStars && frontier.Count > 0)
        {

            centerNode = frontier[0];
            GenerateNeighbours(frontier, ref spawnedStars, centerNode);
            frontier.RemoveAt(0);


        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].InitialisePlanets();
        }




    }

    private void GenerateNeighbours(List<Node> frontier, ref int spawnedStars, Node centerNode)
    {
        OneDimToTwoDim(centerNode.coordinate, rows, out int centerX, out int centerY);

        int mustConnection = Random.Range(0, 8);
        int neighbourIndex = -1;
        float connectionChance = 0.3f;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                neighbourIndex++;
                if (!InsideBounds(x + centerX, y + centerY))
                {
                    continue;
                }
                if (Random.value > connectionChance && neighbourIndex != mustConnection)
                {
                    continue;
                }

                int coord = TwoDimToOneDim(x + centerX, y + centerY, rows);

                if (grid[coord] == null)
                {
                    if (spawnedStars >= numberOfStars)
                    {
                        continue;
                    }
                    //spawn a new star at that neighbouring location and add it to the frontier array. Then connect to it.
                    spawnedStars++;
                    Star newStar = SpawnStar(x + centerX, y + centerY);
                    newStar.index = spawnedStars - 1;

                    stars[spawnedStars - 1] = newStar;
                    grid[coord] = newStar;

                    Node newNode = new Node(coord, centerNode.coordinate, centerNode.steps + 1);
                    InsertNodeBySteps(newNode, frontier);

                    StarConnections.Connect(grid[centerNode.coordinate], newStar);

                }
                else if (coord != centerNode.previousCoordinate)
                {
                    //connect to that star.
                    StarConnections.Connect(grid[centerNode.coordinate], grid[coord]);
                }


            }
        }

    }

    private void InsertNodeBySteps(Node node, List<Node> nodes)
    {
        for(int i = nodes.Count-1;i >= 0; i--)
        {
            if(nodes[i].steps < node.steps || i == 0)
            {
                nodes.Insert(i, node);
                break;
            }
        }
    }

    private Star SpawnStar(int x, int y)
    {
        Star newStar =  Instantiate(starPrefab, new Vector2(x, y) * space + RandomOffset(), Quaternion.identity).GetComponent<Star>();
        newStar.Initialise();


        return newStar;
    }

    int ArrayContains(int[] array,int integer)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i] == integer)
            {
                return i;
            }
        }
        return -1;
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

    bool InsideBounds(int x, int y)
    {
        
        if (x >= 0 && x < collums)
        {
            if (y >= 0 && y < rows)
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
        else
        {
            
            return false;
        }
    }

    private Vector2 RandomOffset(float range = 100f)
    {
        float half = range / 2f;
        return new Vector2(Random.Range(-half, half), Random.Range(-half, half));
    }

    private struct Node
    {
        public int coordinate;
        public int previousCoordinate;
        public int steps;

        public Node(int coordinate, int previousCoordinate, int steps)
        {
            this.coordinate = coordinate;
            this.previousCoordinate = previousCoordinate;
            this.steps = steps;
        }

    }

    public Star RandomStar(bool canBeOccupied = false)
    {
        if (canBeOccupied)
        {
            return stars[Random.Range(0, stars.Length)];
        }
        int iterations = 200;

        for(int i = 0; i < iterations; i++)
        { 

            int roll = Random.Range(0, stars.Length);

            if(stars[roll].factionIndex < 0)
            {
                return stars[roll];
            }
        }
        Debug.LogError("Couldn't find an unhabited star under " + iterations + "iterations");
        return null;
    }

   





}
