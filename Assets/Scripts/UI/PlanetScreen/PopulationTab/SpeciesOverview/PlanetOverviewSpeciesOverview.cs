using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetOverviewSpeciesOverview : MonoBehaviour
{
    public PieChart pieChart;
    public Animator speciesAnimation;
    public TMP_Text speciesTitle;
    public TMP_Text speciesDescription;
    public WedgePress wedgePress;

    private List<Population> populations;
    private Species selectedSpecies;

    private void Start()
    {
        wedgePress.AddToClickEvent(OnPieChartWedgePress);
    }
    public void DisplayDominantPopulation(List<Population> populations)
    {
        if(populations.Count <= 0)
        {
            return;
        }

        int highest = 0;
        int index = 0;
        for(int i = 0; i < populations.Count; i++)
        {
            if(populations[i].size > highest)
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
        selectedSpecies = populations[pieChartWedge.index].species;
        speciesAnimation.SetInteger("SpeciesType", selectedSpecies.animation);
        speciesTitle.text = selectedSpecies.name;
        speciesDescription.text = selectedSpecies.description;
    }

   

    private void UpdatePieChart(List<Population> populations)
    {
        float[] values = new float[populations.Count];
        for(int i = 0; i < values.Length; i++)
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
