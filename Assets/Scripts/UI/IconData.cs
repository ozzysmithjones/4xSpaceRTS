using UnityEngine;
using UnityEngine.UI;

public class IconData : MonoBehaviour
{
    private IconHandler iconHandler;
    public int ID = -1;
    public Image image;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        iconHandler = GetComponentInParent<IconHandler>();
    }

    public void SetImage(Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
    }

    //referenced by the icons button.
    public void OnControlShip()
    {

        if(iconHandler == null)
        {
            iconHandler = GetComponentInParent<IconHandler>();
        }

        Fleet fleet = iconHandler.FindFleet(ID);

        if (fleet == null)
        {
            Debug.LogWarning("couldn't find the fleet to add to the move tool");
            return;
        }

        if (toggle.isOn)
        {
            Master.instance.userInterface.moveFleetTool.AddFleet(fleet);
        }
        else
        {
            Master.instance.userInterface.moveFleetTool.RemoveFleet(fleet);
        }
    }

    public void setToggleOn(bool active)
    {
        toggle.isOn = active;
    }
}
