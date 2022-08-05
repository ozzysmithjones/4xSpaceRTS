using UnityEngine;



public struct PathFindNode<T>
{
    public T breadcrumb;
    public double g, f;
    public ulong iteration;
    public bool inOpen;

    public PathFindNode(T breadcrumb, double g, double f, ulong iteration, bool inOpen)
    {
        this.breadcrumb = breadcrumb;
        this.g = g;
        this.f = f;
        this.iteration = iteration;
        this.inOpen = inOpen;
    }
}


[RequireComponent(typeof(StarGeneration))]
[RequireComponent(typeof(StarShipManager))]
[RequireComponent(typeof(StarConnections))]
public class Star : MonoBehaviour
{
    public PathFindNode<Star> node = new PathFindNode<Star>(null,0,0,0, false);
    public int index;
    public int x, y;

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
    public Empire empire = null;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }


    public void Initialise(Empire empire)
    {
        this.empire = empire;

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

        starGeneration.Generate(this, 5, Random.Range(1, 8), 1f, empire);

        /*
        if (empire != null)
        {
            isColony = true;
            TakeOver(empire, true);
            starGeneration.planets[0].Colonise(empire);

            for (int i = 0; i < 3; i++)
            {
                starConstruction.Build(Master.instance.characters.empires[0].shipTypes[0].prefab, StarConstruction.StarConstructionType.spaceShip);
            }
        }
        */
    }



    public void TakeOver(Empire invader, bool showOuterRim = false)
    {
        RemovePreviousOwnership();

        if (invader == null)
        {
            empire = null;
            starUI.SetUIColor(Color.grey);
            return;
        }

        ApplyInvaderOwnership(invader, showOuterRim);

    }

    private void ApplyInvaderOwnership(Empire invader, bool showOuterRim)
    {

        empire = invader;
        starUI.SetUIColor(empire.flagColor);

        //faction.territory.Add(this);
        empire.territory.AddToTerritory(this, showOuterRim, isColony);

        starUI.SetUIColor(empire.flagColor);

        empire.territory.AddToTerritory(this, showOuterRim, isColony);

        if (!isColony)
        {
            starEconomy.StartEconomy();
        }

        if (empire == Empire.player)
        {
            starVisibility.IncrementFogOfWar(1, 1);
        }

        starEconomy.ApplyResourceproduction(true);
    }

    private void RemovePreviousOwnership()
    {
        if (empire != null)
        {
            starEconomy.ApplyResourceproduction(false);

            if (empire == Empire.player)
            {
                starVisibility.IncrementFogOfWar(-1, 1);
            }

            empire.territory.RemoveFromTerritory(this, false, isColony);
        }
    }

    public void Colonise(Empire empire, int planetIndex = 0)
    {
        isColony = true;
        starGeneration.planets[planetIndex].Colonise(empire);
        empire.territory.colonyStars.Add(this);
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

        Master.instance.userInterface.currentTool.OnInteractStar(this);
    }

    private void OnMouseEnter()
    {
        if (!starVisibility.visibility)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            Master.instance.userInterface.currentTool.OnInteractStar(this);
        }
        else
        {
            Master.instance.userInterface.currentTool.OnHoverStar(this);
        }
    }





}
