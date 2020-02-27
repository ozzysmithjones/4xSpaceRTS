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
    // Start is called before the first frame update
    void Start()
    {

        /*
        master = GetComponent<Master>();
        grid = new Star[collums * rows];
        stars = new Star[numberOfStars];

        //RandomGrid();

        //generating a circle:
        
         for(int i = 0; i < numberOfStars; i++)
         {


             float angle = ((float)i / numberOfStars * 360f) * Mathf.Deg2Rad;
             float distance = 60f;

             Vector2 spawnPoint = new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));

             Instantiate(starPrefab, spawnPoint, transform.rotation);
         }
         */



    }


    public void BreadthFirstGenerate()
    {

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
       

        while (spawnedStars < numberOfStars && frontier.Count > 0){

            Node expansionNode = frontier[0];
            int cX, cY = 0;
            OneDimToTwoDim(expansionNode.coordinate, rows, out cX, out cY);


            //expand
   
            int mustConnection = Random.Range(0,8);
            int i = -1;
            float connectionChance = 0.3f;
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1;y <= 1; y++)
                {
                    //i only increments if we ignore the center.
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                    i++;
                    if (!InsideBounds(x + cX,y + cY))
                    {
                        continue;
                    }
                    if(Random.value > connectionChance && i != mustConnection)
                    {
                        continue;
                    }
                    int coord = TwoDimToOneDim(x + cX, y + cY, rows);

                    if (grid[coord] == null)
                    {
                        if(spawnedStars >= numberOfStars)
                        {
                            continue;
                        }
                        spawnedStars++;
                        Star newStar = SpawnStar(x + cX, y + cY);
                        newStar.index = spawnedStars - 1;
                        stars[spawnedStars-1] = newStar;
                        grid[coord] = newStar;
                        Node newNode = new Node(coord, expansionNode.coordinate, expansionNode.steps + 1);

                        InsertNodeBySteps(newNode, frontier);

                        StarConnections.Connect(grid[expansionNode.coordinate], newStar);

                    }
                    else if(coord != expansionNode.previousCoordinate)
                    {
                        StarConnections.Connect(grid[expansionNode.coordinate], grid[coord]);
                    }


                }
            }

            frontier.RemoveAt(0);


        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].InitialisePlanets();
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



   public void RandomGrid(int factions = 0)
    {
        master = GetComponent<Master>();
        grid = new Star[collums * rows];
        stars = new Star[numberOfStars];

        int[] empirePositions = new int[factions];
        

        bool[] spawnpoints = new bool[collums * rows];
        //create a grid of bools indicating positions that will be spawned on the grid. 
        for (int x = 0; x < collums; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                int index = master.TwoDimToOneDim(x, y, rows);
                if (index < numberOfStars)
                {
                    spawnpoints[index] = true;

                }else
                {
                    spawnpoints[index] = false;
                }
                if (index < factions)
                {
                    empirePositions[index] = index;
                }

            }
        }
        //shuffle the bool array to spawn at random locations.
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            int roll = Random.Range(0, spawnpoints.Length);
            bool original = spawnpoints[i];

            spawnpoints[i] = spawnpoints[roll];
            spawnpoints[roll] = original;

            //shuffle factions positions too.
            int containsI = ArrayContains(empirePositions, i);
            int containsRolled = ArrayContains(empirePositions, roll);

            if (containsI >= 0)
            {

                empirePositions[containsI] = roll;
                //spawnpoints[roll] = rollOriginal;

              
                
            }
            if (containsRolled >= 0)
            {
                empirePositions[containsRolled] = i;
               // spawnpoints[i] = ;

                
            }
            

        }

        //spawn based of the bool array. 
       
        for (int i = 0, starNumber = 0; i < spawnpoints.Length; i++)
        {
            
            if (spawnpoints[i] == true)
            {

                OneDimToTwoDim(i, rows, out int x, out int y);

                Star star = SpawnStar(x, y);
                grid[i] = star;
              //  grid[i].position = i;


                //if there is a faction here, be sure to reference when initialising the star.
                int faction = -1;
                int index = ArrayContains(empirePositions, i);
                if (index >= 0)
                {
                    faction = index;
                    //print("faction = " +faction);
                }
                
                stars[starNumber] = grid[i];
                stars[starNumber].index = starNumber;
                stars[starNumber].Initialise(faction);

                starNumber++;
            }
            //PrintArray(empirePositions);

        }
        //connect the stars
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            if (spawnpoints[i] == true)
            {
                grid[i].starConnections = grid[i].GetComponent<StarConnections>();
                grid[i].starConnections.ConnectToNearestStars(i, 3);

              
            }
        }

        for(int i = 0; i < stars.Length; i++)
        {
            stars[i].InitialisePlanets();
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

    /*
    void PrintArray(int[] array)
    {
        string printed = "";
        for(int i = 0; i < array.Length; i++)
        {
            printed = printed + array[i] + ",";
        }
        print(printed);
    }
    */

    
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


    public List<int> GetNeighbouringTiles(int xy)
    {
        List<int> neighbours = new List<int>();
        OneDimToTwoDim(xy, rows, out int x, out int y);
        for(int horizontal = -1; horizontal <= 1; horizontal++)
        {
            for (int vertical = -1; vertical <= 1; vertical++)
            {
                if(horizontal != 0 || vertical != 0)
                {
                    
                    int neighbour = TwoDimToOneDim(horizontal + x, vertical + y, rows);
                    if (InsideBounds(horizontal + x,vertical + y))
                    {
                        neighbours.Add(TwoDimToOneDim(horizontal + x, vertical + y, rows));
                    }
                    
                }
            }
        }

        if(neighbours.Contains(xy)){
            print("is in neighbours");
        }
        


        return neighbours;
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
