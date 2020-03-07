using UnityEngine;


[RequireComponent(typeof(StarGeneration))]
[RequireComponent(typeof(StarShipManager))]
[RequireComponent(typeof(StarConnections))]
public class Star : MonoBehaviour
{
    public int index;
  //  public int position;
    public int planets;

    
    public LineRenderer starSelect;
    public StarConstruction starConstruction;
    public StarVisibility starVisibility;
    public StarGeneration starGeneration;
    public StarEconomy starEconomy;
    public StarShipManager starShipManager;
    public StarConnections starConnections;
    public StarUI starUI;
    //public StarFogOfWar starFogOfWar;

    //faction:
    public bool isColony = false;
    public int factionIndex = -1;
    private Color factionlessColor = Color.grey;

    // Start is called before the first frame update
    void Start()
    {
       // Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialise(int faction = -1)
    {

        factionIndex = faction;

        starConstruction = GetComponent<StarConstruction>();
        starVisibility = GetComponent<StarVisibility>();
        starGeneration = GetComponent<StarGeneration>();
        starEconomy = GetComponent<StarEconomy>();
        starShipManager = GetComponent<StarShipManager>();
        starConnections = GetComponent<StarConnections>();
        starUI = GetComponent<StarUI>();

        starShipManager.Initialise();
        
    }

    public void InitialisePlanets()
    {
        starVisibility.Initialise();
        starUI.Initialise();
        starConstruction.Initialise();
        starEconomy.Initialise();

        factionlessColor = starUI.factionBorder.color;

        starGeneration.Generate(this, 5, Random.Range(1, 8), 1f, factionIndex);

        if (factionIndex >= 0)
        {
            isColony = true;
            TakeOver(factionIndex, true);
            starGeneration.planets[0].Colonise();

            for (int i = 0; i < 3; i++)
            {
                starConstruction.Build(Master.instance.factions.factions[0].shipTypes[0].prefab, StarConstruction.StarConstructionType.spaceShip);
            }
        }
    }

    

    public void TakeOver(int invader, bool showOuterRim = false)
    {
<<<<<<< Updated upstream

        for(int i = 0; i < starGeneration.planets.Length; i++)
        {
            starGeneration.planets[i].ApplyResourceproduction(false);
        }


        if (factionIndex == 0)
        {
            starVisibility.IncrementFogOfWar(-1, 1);
        }
=======
        RemovePreviousOwnership();
>>>>>>> Stashed changes

        if (invader < 0)
        {
            factionIndex = -1;
            starUI.SetUIColor(Color.grey);
            return;
        }

<<<<<<< Updated upstream
        //print(invader + " is taking over");
=======
        ApplyInvaderOwnership(invader, showOuterRim);

    }

    private void ApplyInvaderOwnership(int invader, bool showOuterRim)
    {
>>>>>>> Stashed changes
        factionIndex = invader;
        Faction faction = Master.instance.factions.factions[factionIndex];

<<<<<<< Updated upstream
        starUI.SetUIColor(faction.flagColor);
       
        //faction.territory.Add(this);
        faction.Influence(this,showOuterRim,isColony);
=======
        starUI.SetUIColor(invadingFaction.flagColor);

        invadingFaction.AddToTerritory(this, showOuterRim, isColony);
>>>>>>> Stashed changes

        if (!isColony)
        {
            starEconomy.StartEconomy();
        }

        if (factionIndex == 0)
        {
            starVisibility.IncrementFogOfWar(1, 1);
        }

        for (int i = 0; i < starGeneration.planets.Length; i++)
        {
            starGeneration.planets[i].ApplyResourceproduction(true);
        }
    }

    private void RemovePreviousOwnership()
    {
        if (factionIndex >= 0)
        {
            for (int i = 0; i < starGeneration.planets.Length; i++)
            {
                starGeneration.planets[i].ApplyResourceproduction(false);
            }


            if (factionIndex == 0)
            {
                starVisibility.IncrementFogOfWar(-1, 1);
            }

            Faction oldFaction = Master.instance.factions.factions[factionIndex];
            oldFaction.RemoveFromTerritory(this, false, isColony);

        }
    }

    public void Colonise(int planetIndex = 0)
    {

        starGeneration.planets[planetIndex].Colonise();
    }

    public void SetSelector(bool active, Color color)
    {
        /*
        starSelect.gameObject.SetActive(active);
        starSelect.startColor = color;
        starSelect.endColor = color;
        */
    }

    private void OnMouseDown()
    {

        if (!starVisibility.visibility)
        {
            return;
        }

        Master.instance.userInterface.currentTool.OnInteract(this);
    }

    private void OnMouseOver()
    {
        if (!starVisibility.visibility)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            Master.instance.userInterface.currentTool.OnInteract(this);
        }
        else
        {
            Master.instance.userInterface.currentTool.OnHover(this);
        }
    }





}
