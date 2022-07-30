using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpandTool : Tool
{

    public override void OnSelected()
    {

        base.OnSelected();

        List<Star> outerRim = Empire.player.territory.outerRim;

        for (int i = 0; i < outerRim.Count; i++)
        {
            if (outerRim[i].empire == null || outerRim[i].empire == Empire.player)
            {
                outerRim[i].SetSelector(true, Color.white);
            }
        }

    }

    public override void OnDeselected()
    {
        base.OnDeselected();

        List<Star> outerRim = Empire.player.territory.outerRim;

        for (int i = 0; i < outerRim.Count; i++)
        {
            outerRim[i].SetSelector(false, Color.white);
        }
    }

    public override void OnInteractStar(Star star)
    {
        base.OnInteractStar(star);

        if (!star.starConnections.IsConnectedToEmpire(Empire.player) || star.empire != null)
        {
            return;
        }

        star.SetSelector(false, Color.white);
        star.TakeOver(Empire.player, true);
    }




}
