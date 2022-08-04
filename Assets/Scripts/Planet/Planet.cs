using UnityEngine;




[RequireComponent(typeof(PlanetTexture))]
public class Planet : MonoBehaviour
{
    public Biome biome { get; private set; }
    public PlanetTexture planetTexture;
    public Star star;
    public PlanetColony planetColony;
    public PlanetUI planetUI;

    public bool isColony = false;


    // Start is called before the first frame update
    void Start()
    {
        planetTexture = GetComponent<PlanetTexture>();
    }

    public void Initialise(Star planetsStar)
    {
        star = planetsStar;
        planetTexture = GetComponent<PlanetTexture>();
        planetTexture.Initialise();

        planetColony = GetComponent<PlanetColony>();
        planetColony.planet = this;

        planetUI = GetComponent<PlanetUI>();
        planetUI.planet = this;
    }



    public void Colonise(Empire empire)
    {
        isColony = true;
        planetColony.Colonise(empire);
    }


    public void SetBiome(Biome biome)
    {
        this.biome = biome;
        planetTexture.SetValues(biome.planetTextureData);
        planetTexture.Generate();

        int[] newResources = biome.GetRandomResourceAmounts();
        int[] resourceProduction = star.starEconomy.totalResourceProduction.amounts;

        for (int i = 0; i < newResources.Length; i++)
        {
            resourceProduction[i] += newResources[i];
        }
    }
}
