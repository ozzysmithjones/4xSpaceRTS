using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpandTool : Tool
{
    
    public override void OnSelected()
    {
        
        base.OnSelected();
        
        for(int i = 0; i < Master.instance.characters.factions[0].outerRim.Count; i++)
        {
            if (Master.instance.characters.factions[0].outerRim[i].factionIndex <= 0)
            {
                Master.instance.characters.factions[0].outerRim[i].SetSelector(true, Color.white);
            }
        }

    }

    public override void OnDeselected()
    {
        base.OnDeselected();

        for (int i = 0; i < Master.instance.characters.factions[0].outerRim.Count; i++)
        {
            Master.instance.characters.factions[0].outerRim[i].SetSelector(false, Color.white);
        }
    }

    public override void OnInteract(Star star)
    {
        base.OnInteract(star);

        if (!star.starConnections.IsConnectedToFaction(0) || star.factionIndex != -1)
        {
            return;
        }

        star.SetSelector(false, Color.white);
        star.TakeOver(0,true);

    }




}
