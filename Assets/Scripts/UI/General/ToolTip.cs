using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    private bool showing = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        showing = true;
        Master.instance.userInterface.toolTipSystem.ActivateToolTip(text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showing = false;
        Master.instance.userInterface.toolTipSystem.DeActivateToolTip();
    }

    public void SetTextAndView(string text)
    {
        this.text = text;
        showing = true;
        Master.instance.userInterface.toolTipSystem.ActivateToolTip(text);
    }
    public void SetText(string text)
    {
        this.text = text;
        if (showing)
        {
            Master.instance.userInterface.toolTipSystem.UpdateText(text);
        }
    }
    public void Clear()
    {
        if (showing)
        {
            showing = false;
            Master.instance.userInterface.toolTipSystem.DeActivateToolTip();
        }
    }

}
