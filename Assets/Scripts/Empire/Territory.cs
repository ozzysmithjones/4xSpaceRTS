using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Territory
{
    [System.NonSerialized] private Empire empire;

    //territory control
    public List<Star> colonyStars = new List<Star>();
    public List<Star> stars = new List<Star>();
    public List<Star> outerRim = new List<Star>();

    public void Init(Empire empire)
    {
        this.empire = empire;
    }

    public void AddToTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {
        outerRim.Remove(star);

        if (stars.Contains(star))
        {
            return;
        }
        stars.Add(star);

        if (colony)
        {
            colonyStars.Add(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();

        for (int i = 0; i < connectedStars.Count; i++)
        {
            if (!outerRim.Contains(connectedStars[i]) && connectedStars[i].empire != empire)
            {
                outerRim.Add(connectedStars[i]);
            }
        }
    }

    public void RemoveFromTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {
        stars.Remove(star);

        if (colony)
        {
            colonyStars.Remove(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();

        for (int i = 0; i < connectedStars.Count; i++)
        {
            if (outerRim.Contains(connectedStars[i]) && !connectedStars[i].starConnections.IsConnectedToEmpire(empire))
            {
                outerRim.Remove(connectedStars[i]);
            }
        }
    }


    public void RandomlyExpand(int lowest = 3, int highest = 8)
    {
        int length = Random.Range(lowest, highest);
        for (int i = 0; i < length; i++)
        {
            if (outerRim.Count <= 0)
            {
                break;
            }
            int index = Random.Range(0, outerRim.Count);

            if (outerRim[index].empire == null)
            {
                outerRim[index].TakeOver(empire);
            }
            else
            {
                outerRim.RemoveAt(index);
                i--;
            }
        }
    }
}
