using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ToolTipSystem : MonoBehaviour
{

    public TMP_Text toolTipText;
    private string text;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
    public void ActivateToolTip(string text)
    {
        gameObject.SetActive(true);
        toolTipText.text = text;
    }
    public void UpdateText(string text)
    {
        this.text = text;
        if (gameObject.activeSelf)
        {
            toolTipText.text = text;
        }
    }

    public void DeActivateToolTip()
    {
        toolTipText.text = ".";
        gameObject.SetActive(false);
    }
}
