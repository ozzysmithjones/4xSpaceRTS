using UnityEngine;

public class StarConstruction : MonoBehaviour
{
    private Star star;
    public GameObject emptyFleetPrefab;
    public Transform visuals;
    public enum StarConstructionType { fleet, spaceShip, spaceStation }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
        }
        if (type == StarConstructionType.spaceShip)
        {
            SpaceShip spaceShip = spawned.GetComponent<SpaceShip>();
            spaceShip.Initialise(Master.instance.characters.factions[star.factionIndex].flagColor);
            int fleetIndex = star.starShipManager.GetSmallestFleet(star.factionIndex, true);

            if (fleetIndex >= 0)
            {
                star.starShipManager.fleets[fleetIndex].AddShip(spaceShip);
                spaceShip.transform.position = (Vector3)Random.insideUnitCircle * 1.5f + spaceShip.transform.position;
                return null;
            }
            else
            {
                Fleet navigator = Instantiate(emptyFleetPrefab, transform.position, transform.rotation).GetComponent<Fleet>();
                navigator.AddShip(spaceShip);
                InitialiseFleet(navigator, star);
                return navigator;

            }
        }
        if (type == StarConstructionType.fleet)
        {
            Fleet navigator = spawned.GetComponent<Fleet>();
            InitialiseFleet(navigator, star);
            return navigator;

        }
        return null;
    }

    private void InitialiseFleet(Fleet fleet, Star star)
    {
        fleet.star = star;
        fleet.AddToFaction(star.factionIndex);
        star.starShipManager.Entry(fleet);
    }
}
