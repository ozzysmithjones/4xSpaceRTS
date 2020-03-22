using UnityEngine;

public class PlanetOverview : MonoBehaviour
{

    public Planet planet;
    public PlanetOverviewDescription description;
    public PlanetOverviewAlreadyBuilt alreadyBuilt;
    public PlanetOverviewBuildQueue buildQueue;
    public BuildMenu buildMenu;

    public PlanetOverviewSpeciesOverview speciesOverview;


    private void Awake()
    {
        alreadyBuilt.planetOverview = this;
        buildQueue.planetOverview = this;
        buildMenu.planetOverview = this;
    }

    public void Overview(Planet newPlanet)
    {

        planet = newPlanet;

        buildQueue.UpdateQueueChange(planet.planetColony.buildQueue);
        planet.planetColony.ListenToBuildQueue(buildQueue.UpdateQueueChange, true);

        speciesOverview.DisplayDominantPopulation(planet.planetColony.populations);
        speciesOverview.OnPopulationChange(planet.planetColony.populations);
        planet.planetColony.ListenToPopulation(speciesOverview.OnPopulationChange, true);


        alreadyBuilt.UpdateAllQuantities();
        description.UpdateDescription(newPlanet);
    }

    public void Open(Planet _planet)
    {
        Master.instance.userInterface.planetOverviewOpen = true;
        gameObject.SetActive(true);
        Overview(_planet);

    }

    public void Close()
    {
        if (planet != null)
        {
            planet.planetColony.ListenToBuildQueue(buildQueue.UpdateQueueChange, false);
            planet.planetColony.ListenToPopulation(speciesOverview.OnPopulationChange, false);
        }
        Master.instance.userInterface.planetOverviewOpen = false;
        gameObject.SetActive(false);
    }
}
