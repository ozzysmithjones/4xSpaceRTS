using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChangeColor : MonoBehaviour
{
    public Toggle toggle;
    public Text text;
    public Color off;
    public Color on;

    public void ChangeColor()
    {
        text.color = toggle.isOn ? on : off;
    }
}
