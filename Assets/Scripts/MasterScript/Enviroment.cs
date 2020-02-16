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
    public float space = 20f;
    public int numberOfStars = 100;
    public int collums = 40;
    public int rows = 40;

    public Star[] grid;
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
            bool rollOriginal = spawnpoints[roll];

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

                GameObject star = Instantiate(starPrefab, new Vector2(x, y) * space + RandomOffset(), Quaternion.identity);
                grid[i] = star.GetComponent<Star>();
                grid[i].position = i;

                //if there is a faction here, be sure to reference when initialising the star.
                int faction = -1;
                int index = ArrayContains(empirePositions, i);
                if ( index >= 0)
                {
                    faction = index;
                    //print("faction = " +faction);
                }
                
                stars[starNumber] = grid[i];
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

    private Vector2 RandomOffset(float range = 0f)
    {
        float half = range / 2f;
        return new Vector2(Random.Range(-half, half), Random.Range(-half, half));
    }

    //returns a random star in the game, possibly for a spawn point or something.
    public Star RandomStar(bool includeAlreadyTaken = false)
    {
        Star star = stars[Random.Range(0, stars.Length)];

        if (star.factionIndex != -1)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                star = stars[Random.Range(0, stars.Length)];
                if (star.factionIndex != -1)
                {
                    break;
                }
            }
        }
        return star;
        
    }





}
