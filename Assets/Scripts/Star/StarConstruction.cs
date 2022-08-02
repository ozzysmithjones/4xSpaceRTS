using UnityEngine;

public class StarConstruction : MonoBehaviour
{
    private Star star;
    public GameObject emptyFleetPrefab;
    public Transform visuals;
    public enum StarConstructionType { fleet, spaceShip, spaceStation }

    public void Initialise()
    {
        star = GetComponent<Star>();
    }

    public Fleet Build(GameObject prefab, StarConstruction.StarConstructionType type)
    {

        GameObject spawned = Instantiate(prefab, transform.position, transform.rotation);
        spawned.transform.SetParent(visuals);

        if (type == StarConstructionType.spaceStation)
        {
            star.starVisibility.AddStaticObject(spawned);

        }else if (type == StarConstructionType.spaceShip)
        {
            SpaceShip spaceShip = spawned.GetComponent<SpaceShip>();
            spaceShip.Initialise(star.empire.flagColor);
            Fleet fleet = star.starShipManager.GetSmallestFleet(star.empire, spaceShip.fleetType);

            if (fleet != null)
            {
                fleet.AddShip(spaceShip);
                spaceShip.transform.position = (Vector3)Random.insideUnitCircle * 1.5f + spaceShip.transform.position;
                return fleet;
            }
            else
            {
                fleet = Instantiate(emptyFleetPrefab, transform.position, transform.rotation).GetComponent<Fleet>();
                fleet.AddShip(spaceShip);
                fleet.type = spaceShip.fleetType;
                fleet.empire = null;
                InitialiseFleet(fleet, star);
                return fleet;
            }
        }else if (type == StarConstructionType.fleet)
        {
            Fleet fleet = spawned.GetComponent<Fleet>();
            InitialiseFleet(fleet, star);
            return fleet;
        }

        return null;
    }

    private void InitialiseFleet(Fleet fleet, Star star)
    {
        fleet.star = star;
        star.empire.military.AddFleet(fleet);
        star.starShipManager.Add(fleet);
    }
}
