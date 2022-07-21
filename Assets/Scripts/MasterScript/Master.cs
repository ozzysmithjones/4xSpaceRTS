using System.Collections.Generic;
using UnityEngine;

//the master is devided into three scripts: the Enviroment, the Factions and the Interface, these will interact with each other. 
[RequireComponent(typeof(Enviroment))]
[RequireComponent(typeof(Characters))]
[RequireComponent(typeof(Interface))]

//Variety makes different building types easily accessible to all scripts. This process is being depreicated to allow empires to have their own unique set of ships and structures. 
[RequireComponent(typeof(Variety))]
public class Master : MonoBehaviour
{

    public float frameRate = 1f;
    public float highestFrameRate = 0.0f;
    public float lowestFrameRate = 60f;

    public static Master instance;
    public Variety variety;
    public Enviroment enviroment;
    public Characters characters;
    public Interface userInterface;


    public int seed = 0;
    public const int collums = 50;
    public const int rows = 50;

    PathFindAlgorithm pathFindAlgorithm;


    // Start is called before the first frame update
    void Awake()
    {
        Random.InitState(seed);
        Singleton();

        enviroment = GetComponent<Enviroment>();
        characters = GetComponent<Characters>();
        userInterface = GetComponent<Interface>();
        variety = GetComponent<Variety>();

        variety.Initialise();

        enviroment.BreadthFirstGenerate(collums, rows);
        characters.SpawnFactions(variety.builtShips, variety.builtStructures);

        pathFindAlgorithm = new PathFindAlgorithm();

        //set the main camera to be above the players home world.
        Vector3 position = characters.factions[0].territory[0].transform.position;
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
        if (frameRate > highestFrameRate && Time.time > 3f)
        {
            highestFrameRate = frameRate;
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



    public List<Star> PathFind(Star start, Star end)
    {
        return pathFindAlgorithm.Path(start, end);
    }






}
