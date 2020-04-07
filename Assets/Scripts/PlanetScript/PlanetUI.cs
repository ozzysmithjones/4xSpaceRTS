using UnityEngine;

public class PlanetUI : MonoBehaviour
{
    public Planet planet;
       
    private void OnMouseEnter()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Master.instance.userInterface.currentTool.OnInteractPlanet(planet);
        }
        else
        {
            Master.instance.userInterface.currentTool.OnHoverPlanet(planet);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("interact planet");
        if (!Master.instance.userInterface.moveFleetTool.active)
        {
            Master.instance.userInterface.OpenPlanetOverview(planet);
        }
        else
        {
            Master.instance.userInterface.currentTool.OnInteractPlanet(planet);
        }
    }

}
