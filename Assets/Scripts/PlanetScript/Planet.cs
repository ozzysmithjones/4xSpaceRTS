using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(PlanetTexture))]
public class Planet : MonoBehaviour
{
    public BiomeType biomeType;
    public PlanetTexture planetTexture;
    public Star star;
    public PlanetColony planetColony;
    public PlanetUI planetUI;

    public bool isColony = false;

    //how much the colony produces:
    public Resources resourceProduction = new Resources();


    // Start is called before the first frame update
    void Start()
    {
        planetTexture = GetComponent<PlanetTexture>();
    }

    // Update is called once per frame
    void Update()
    {
        
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



    public void Colonise()
    {
        isColony = true;
        planetColony.Colonise();
      
    }


    public void SetBiome(Biome biome)
    {
        this.biomeType = biome.biomeType;
        planetTexture.SetValues(biome.planetTextureData);
        planetTexture.Generate();


        resourceProduction.amounts = biome.GetRandomResourceAmounts();

    }

    public void ApplyResourceproduction(bool positive)
    {
        if(star.factionIndex < 0)
        {
            return;
        }
        Faction faction = Master.instance.factions.factions[star.factionIndex];

        for(int i = 0; i < resourceProduction.amounts.Length; i++)
        {
            faction.ImproveResourceProduction((ResourceType)i, resourceProduction.amounts[i] * (positive ? 1 : -1));
        }

    }

    public void ImproveResourceproduction(ResourceType resourceType, int amount)
    {
        resourceProduction.amounts[(int)resourceType] += amount;
        Master.instance.factions.factions[star.factionIndex].ImproveResourceProduction(resourceType, amount);
    }

    


}
