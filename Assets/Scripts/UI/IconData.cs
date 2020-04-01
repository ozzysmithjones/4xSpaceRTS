using UnityEngine;
using UnityEngine.UI;

public class IconData : MonoBehaviour
{
    private IconHandler iconHandler;
    public int ID = -1;
    public Image image;

    private Toggle toggle;

    private bool fleetCanBeControlled = false;
    private void Start()
    {

        //image = GetComponent<Image>();
        toggle = GetComponent<Toggle>();
        iconHandler = GetComponentInParent<IconHandler>();
        //toggle.interactable = false;
    }

    public void SetImage(Sprite sprite, bool interactable = false, int faction = -1)
    {
        image.sprite = sprite;

        if (faction >= 0 && interactable)
        {
            image.color = Master.instance.characters.factions[faction].flagColor;
        }



        if (toggle == null)
        {
            //image = GetComponent<Image>();
            toggle = GetComponent<Toggle>();
            //toggle.isOn = false;
        }

        if (!gameObject.activeSelf)
        {
            return;
        }
        else
        {
            fleetCanBeControlled = interactable;
            toggle.interactable = interactable;
        }

        // Debug.Log(interactable);



        //toggle.enabled = interactable;
        //fleetCanBeControlled = interactable;
    }

    public void OnControlShip()
    {
        if (!fleetCanBeControlled)
        {
            return;
        }


        if (iconHandler == null)
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
