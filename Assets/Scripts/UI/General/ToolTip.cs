using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Master.instance.userInterface.toolTipSystem.ActivateToolTip(text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Master.instance.userInterface.toolTipSystem.DeActivateToolTip();
    }

    public void SetTextAndView(string text)
    {
        this.text = text;
        Master.instance.userInterface.toolTipSystem.ActivateToolTip(text);
    }
    public void SetText(string text)
    {
        this.text = text;
        Master.instance.userInterface.toolTipSystem.UpdateText(text);
    }
}
