using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchOptionUI : MonoBehaviour
{

    public TMP_Text title;
    public ToolTip toolTip;
    public Image image;
    public Image backGround;
    public void Initialise(ResearchQueueItem researchQueueItem,Color backgroundColor)
    {
        gameObject.SetActive(true);
        title.text = researchQueueItem.name;
        toolTip.SetText(researchQueueItem.description);
        image.sprite = researchQueueItem.sprite;
        backGround.color = backgroundColor;
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }
}
