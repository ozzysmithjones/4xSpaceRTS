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
    public void SetText(string text)
    {
        gameObject.SetActive(true);
        toolTipText.text = text;
    }

    public void ClearText()
    {
        toolTipText.text = ".";
        gameObject.SetActive(false);
    }
}
