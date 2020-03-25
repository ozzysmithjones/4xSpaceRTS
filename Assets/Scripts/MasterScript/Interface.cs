using System;
using UnityEngine;
using UnityEngine.UI;


public class Interface : MonoBehaviour
{
    
    public ToolTipSystem toolTipSystem;
    public Tool currentTool;

    private Tool[] tools = new Tool[4];

    public Text[] resourcesText;
    public bool planetOverviewOpen = false;
    public PlanetOverview planetOverview;

    private bool researchOverviewOpen = false;
    public ResearchOverview researchOverview;

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

    public void OpenResearchOverview(Research research)
    {
        researchOverview.Open(research);
    }


    public bool ToggleResearchOverview()
    {
        researchOverviewOpen = !researchOverviewOpen;
        if (researchOverviewOpen)
        {
            researchOverview.Open(Master.instance.characters.factions[0].research);
        }
        else
        {
            researchOverview.Close();
        }
        return researchOverviewOpen;
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
