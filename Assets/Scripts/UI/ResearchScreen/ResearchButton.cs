using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchButton : MonoBehaviour
{
    public void Press()
    {
        Master.instance.userInterface.OpenResearchOverview(Master.instance.characters.factions[0].research);
    }
}
