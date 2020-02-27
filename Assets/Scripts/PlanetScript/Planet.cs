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


    public void SetBiome(BiomeType biomeType,PlanetTextureData planetTextureData)
    {
        this.biomeType = biomeType;
        planetTexture.SetValues(planetTextureData);
        planetTexture.Generate();


    }


}
