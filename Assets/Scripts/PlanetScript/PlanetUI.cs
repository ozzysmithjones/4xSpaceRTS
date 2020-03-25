using UnityEngine;

public class PlanetUI : MonoBehaviour
{
    public Planet planet;



    public void OnMouseDown()
    {
        // if (planet.isColony)
        // {

        //}
        Master.instance.userInterface.OpenPlanetOverview(planet);
    }
}
