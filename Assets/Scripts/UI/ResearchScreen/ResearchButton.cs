using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoBehaviour
{
    public Text text;
    public Color onColor;
    public Color offColor;
    private void Start()
    {
        
    }
    public void Press()
    {
        ChangeColor(Master.instance.userInterface.ToggleResearchOverview());
    }

    public void ChangeColor(bool isOn)
    {
        text.color = isOn ? onColor : offColor;
    }
}
