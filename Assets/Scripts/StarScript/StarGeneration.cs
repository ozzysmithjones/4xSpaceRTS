using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGeneration : MonoBehaviour
{
    public Transform visuals;
    public GameObject planetPrefab;
    public float spaceBetweenPlanets = 2f;
    public float spaceBetweenVoid = 0.5f;
    public BiomeGradient groundGradient;
    public Planet[] planets;



    //private Visibility visibility;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Generate(Star star,int radius,int numberOfPlanets, float colony = 0.25f, int faction = -1)
    {
        planets = new Planet[numberOfPlanets];
        radius = Mathf.Max(radius, numberOfPlanets);

        bool[] planetPositions = new bool[radius];

        //make array
        for(int i = 0; i < planetPositions.Length; i++)
        {
            if (i < numberOfPlanets)
            {
                planetPositions[i] = true;
            }
            else
            {
                planetPositions[i] = false;
            }
        }

        //shuffle array.
        for (int i = 0; i < planetPositions.Length; i++)
        {
            int roll = Random.Range(0, planetPositions.Length);
            bool first = planetPositions[i];
            bool second = planetPositions[roll];

            planetPositions[i] = second;
            planetPositions[roll] = first;

        }

        

        int PlanetIndex = 0;
        //generate planets and other objects
        for (int i = 0; i < planetPositions.Length; i++)
        {
            if (planetPositions[i])
            {
                float space = spaceBetweenPlanets;

                //if last ploanet position was null, then space is space between void.Otherwise it's space between planets.
                if(i - 1 >= 0)
                {
                    if(!planetPositions[i-1])
                    {
                        space = spaceBetweenVoid;
                    }
                }
                //place planet horizontally. 
                Vector2 position = (Vector2)transform.position + new Vector2((float)i * space, 0) + new Vector2(transform.localScale.x,0);
                Planet planet = Instantiate(planetPrefab, position, transform.rotation).GetComponent<Planet>();
                planet.transform.SetParent(visuals);

                if(PlanetIndex >= planets.Length)
                {
                    Debug.LogError("index really is out of range, planets length = " + planets.Length + " index = " + PlanetIndex + " positions length" + planetPositions.Length);

                }
                planets[PlanetIndex] = planet;
                PlanetIndex++;

                planet.Initialise(star);

                if (planet == null)
                {
                    print("No planet");

                }
                else if (planet.planetTexture == null)
                {
                    print("no texture");
                }
                
                Color sea = (groundGradient.GetColor((float)i / ((float)planetPositions.Length - 1)) * new Color(0.8f, 0.8f, 0.8f, 1f));
                Color ground = groundGradient.GetColor((float)i / (float)planetPositions.Length);
                if (faction >= 0 && PlanetIndex == 1)
                {
                    if (Master.instance.factions.factions.Count <= 0)
                    {
                        Debug.LogError("no factions");
                    }else if (faction >= Master.instance.factions.factions.Count)
                    {
                        Debug.LogError("index is too big :"+ faction);
                        for(int x = 0; x < Master.instance.factions.factions.Count; x++)
                        {
                            Debug.Log("faction : " + Master.instance.factions.factions[x].factionIndex);
                        }
                    }
                    else { 
                        ground = Master.instance.factions.factions[faction].homePlanet.color;
                       
                    }
                    
                }
                planet.SetBiome(groundGradient.GetPoint((float)i / ((float)planetPositions.Length-1f)).biomeType, new PlanetTextureData(sea, ground, new Vector2(Random.value, Random.value), 5f));

                //rotate the planet around the sun. 
                planet.transform.RotateAround(transform.position, Vector3.forward, Random.value * 360f);
            }

        }

        //make all the children invisible.
       
       // Master.instance.userInterface.SetMapUI(true);


    }

    Color GetSeaColor(Color groundColor, bool water = false)
    {


        return Color.white;
    }
   
}
