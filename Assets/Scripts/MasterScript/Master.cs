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
   
    public float frameRate = 1f;
    public float lowestFrameRate = 60f;

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
        Singleton();
        
        pathFindAlgorithm = new PathFindAlgorithm();
        enviroment = GetComponent<Enviroment>();
        factions = GetComponent<Factions>();
        userInterface = GetComponent<Interface>();
        variety = GetComponent<Variety>();

        variety.Initialise();

        enviroment.BreadthFirstGenerate();

        factions.SpawnFactions(variety.builtShips,variety.builtStructures);


        //set the main camera to be above the players home world.
        Vector3 position = factions.factions[0].territory[0].transform.position;
        Camera.main.transform.position = new Vector3(position.x, position.y, -10);

    }

    // Update is called once per frame
    void Update()
    {
        frameRate = 1f / Time.deltaTime;
        if (frameRate < lowestFrameRate && Time.time > 3f)
        {
            lowestFrameRate = frameRate;
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

    public List<int> PathFind(int start, int end, int[] factionsAllowed = null, float maxLength = 0f)
    {
       return pathFindAlgorithm.PathFind(start, end,factionsAllowed,maxLength);
    }


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
  

   

    
   
}
