using UnityEngine;

public class StarGeneration : MonoBehaviour
{
    public Transform visuals;
    public GameObject planetPrefab;
    public float spaceBetweenPlanets = 2f;
    public float spaceBetweenVoid = 0.5f;
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


    public void Generate(Star star, int radius, int numberOfPlanets, float colony = 0.25f, Empire empire = null)
    {
        planets = new Planet[numberOfPlanets];
        radius = Mathf.Max(radius, numberOfPlanets);

        bool[] planetPositions = new bool[radius];

        //make array
        for (int i = 0; i < planetPositions.Length; i++)
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


        int planetIndex = 0;
        //generate planets and other objects
        for (int i = 0; i < planetPositions.Length; i++)
        {
            if (planetPositions[i])
            {
                float space = spaceBetweenPlanets;

                //if last ploanet position was null, then space is space between void.Otherwise it's space between planets.
                if (i - 1 >= 0)
                {
                    if (!planetPositions[i - 1])
                    {
                        space = spaceBetweenVoid;
                    }
                }
                Planet planet = SpawnPlanet(star, ref planetIndex, i, space);
                SetBiomeOfPlanet(planetPositions.Length, i, planet);

            }

        }
    }

    private void SetBiomeOfPlanet(int furthestPlanetPosition, int planetPosition, Planet planet)
    {

        Biome biome = Master.instance.variety.biomeGradient.GetBiomeAtPoint((float)planetPosition / (furthestPlanetPosition - 1));
        planet.SetBiome(biome);
    }

    private Planet SpawnPlanet(Star star, ref int planetIndex, int planetPosition, float space)
    {
        //place planet horizontally. 
        Vector2 position = (Vector2)transform.position + new Vector2((float)planetPosition * space, 0) + new Vector2(transform.localScale.x, 0);
        Planet planet = Instantiate(planetPrefab, position, transform.rotation).GetComponent<Planet>();
        planet.transform.SetParent(visuals);

        planets[planetIndex] = planet;
        planetIndex++;

        planet.Initialise(star);

        if (planet == null)
        {
            print("No planet");

        }
        else if (planet.planetTexture == null)
        {
            print("no texture");
        }
        //rotate the planet around the sun. 
        planet.transform.RotateAround(transform.position, Vector3.forward, Random.value * 360f);

        return planet;
    }


}
