using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class PlanetProductionDescription : MonoBehaviour
{
    public TextMeshProUGUI description;
    private Planet planet;

    public void SetPlanet(Planet planet)
    {
        this.planet = planet;
    }


    private void Update()
    {
        if (planet != null)
        {
            StringBuilder stringBuilder = new StringBuilder("Planet production per Pop:\n");
            int numResourceTypes = Enum.GetValues(typeof(ResourceType)).Length;

            for (int i = 0; i < numResourceTypes; ++i)
            {
                stringBuilder.Append(((ResourceType)i).ToString());
                stringBuilder.Append(": ");
                stringBuilder.Append(planet.planetColony.resourceBonus.amounts[i]);
                stringBuilder.AppendLine();
            }

            description.text = stringBuilder.ToString();
        }
    }
}
