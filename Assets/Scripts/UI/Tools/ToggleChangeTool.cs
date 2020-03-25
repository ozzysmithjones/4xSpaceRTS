using UnityEngine;
using UnityEngine.UI;

public class ToggleChangeTool : MonoBehaviour
{
    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
    }
    public void ChangeTool(int tool)
    {
        bool on = toggle.isOn;

        if (on)
        {
            Master.instance.userInterface.SetTool(tool);
        }
        else
        {
            Master.instance.userInterface.SetTool(0);
        }
    }
}
