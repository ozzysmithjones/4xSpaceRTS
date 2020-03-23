using UnityEngine;
using UnityEngine.UI;


public class Interface : MonoBehaviour
{
    //PLayer will have a selection of tools: expand, build, and control ships. When a player clicks on a star 
    //with a tool selected, depending on the tool a different interaction will occur. Each tool will have an on selected event and 
    //an on interact event.

    //another system is the build system. A build panel is opened and the player can click on an item to place in the world.When
    //the player clicks on an item, the mouse changes it's picture to a small representation of the item.When the player clicks 
    //on a star with the item selected, it's placed there in the world.

    //when an item is placed, if it's a space station it is added to the structures array on the fleet manager. If it's a fleet it is added 
    //to the fleet managers fleets array. If it's just a ship it is added on the fleet manager to any fleet in the system otherwise it 
    //creates a new fleet with that ship.

    public ToolTipSystem toolTipSystem;
    public Tool currentTool;

    private Tool[] tools = new Tool[4];

    public Text[] resourcesText;
    public bool planetOverviewOpen = false;
    public PlanetOverview planetOverview;





    public MoveFleetTool moveFleetTool;
    // Start is called before the first frame update
    void Start()
    {


        //SetMapUI(true);
        tools[0] = new Tool();
        tools[1] = new ExpandTool();
        tools[2] = new BuildTool();
        tools[3] = new MoveFleetTool();


        LineRenderer moveFleetToolLine = GetComponent<LineRenderer>();
        moveFleetTool = tools[3] as MoveFleetTool;
        moveFleetTool.lineRenderer = moveFleetToolLine;


    }

    // Update is called once per frame
    void Update()
    {

    }



    public void SetTool(int tool)
    {
        currentTool.OnDeselected();
        currentTool = tools[tool];
        currentTool.OnSelected();

        //Debug.Log(tool.ToString() + "tool c);
    }
    public void OpenPlanetOverview(Planet planet)
    {
        planetOverview.Open(planet);
    }

    public void ClosePlanetOverview()
    {
        planetOverview.Close();
    }


    public void SetMapUI(bool visibility)
    {
        Star[] stars = Master.instance.enviroment.stars;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].starUI.SetUIVisibility(visibility);
        }

        int layerHidden = Camera.main.cullingMask;
        if (visibility)
        {
            layerHidden = 1 << LayerMask.NameToLayer("PlanetUI");
        }
        else
        {

            layerHidden = 1 << LayerMask.NameToLayer("MapUI");
        }

        Camera.main.cullingMask = ~(layerHidden);
        //print(Camera.main.cullingMask);
    }


}
