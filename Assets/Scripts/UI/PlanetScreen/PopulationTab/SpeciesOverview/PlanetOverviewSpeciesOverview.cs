using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlanetOverviewSpeciesOverview : MonoBehaviour
{
    public PieChart pieChart;
    public Animator speciesAnimation;
    public TMP_Text speciesTitle;
    public TMP_Text speciesDescription;
    public WedgeMouseInput wedgePress;

    private List<Population> populations;
    private Species selectedSpecies;

    public ToolTip populationToolTip;

    private void Start()
    {
        wedgePress.AddToMouseClickEvent(OnPieChartWedgePress,true);
        wedgePress.AddToMouseEnterEvent(OnPieChartWedgeEnter, true);
    }
    public void DisplayDominantPopulation(List<Population> populations)
    {
        if (populations.Count <= 0)
        {
            return;
        }

        int highest = 0;
        int index = 0;
        for (int i = 0; i < populations.Count; i++)
        {
            if (populations[i].size > highest)
            {
                highest = populations[i].size;
                index = i;
            }
        }

        selectedSpecies = populations[index].species;
        speciesAnimation.SetInteger("SpeciesType", selectedSpecies.animation);
        speciesTitle.text = selectedSpecies.name;
        speciesDescription.text = selectedSpecies.description;

    }

    public void OnPopulationChange(List<Population> populations)
    {
        this.populations = populations;
        UpdatePieChart(populations);
    }

    private void OnPieChartWedgePress(PieChartWedge pieChartWedge)
    {
      
        speciesAnimation.SetInteger("SpeciesType", selectedSpecies.animation);
        speciesTitle.text = selectedSpecies.name;
        speciesDescription.text = selectedSpecies.description;
    }

    private void OnPieChartWedgeEnter(PieChartWedge pieChartWedge)
    {
        selectedSpecies = populations[pieChartWedge.index].species;
        populationToolTip.SetText(selectedSpecies.name + " at " + populations[pieChartWedge.index].size + " Population");
    }




    private void UpdatePieChart(List<Population> populations)
    {
        float[] values = new float[populations.Count];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = populations[i].size;
        }
        Color[] colors = new Color[populations.Count];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = populations[i].species.color;
        }
        pieChart.UpdateWedges(values, colors);
    }



}
