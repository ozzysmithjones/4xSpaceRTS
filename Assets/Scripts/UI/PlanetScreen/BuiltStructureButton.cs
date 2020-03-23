using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuiltStructureButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuildQueueItemDescription buildQueueItemDescription;

    //the class of building that was built here
    public TextMeshProUGUI structureName;
    public TextMeshProUGUI structureQuantity;
    public BuiltStructure builtStructure;
    public int quantity = 0;


    private static int classIndex = 0;

    private void Start()
    {
        builtStructure = Master.instance.characters.factions[0].structureTypes[classIndex];
        structureName.text = builtStructure.name;
        structureQuantity.text = quantity.ToString();
        classIndex++;

    }


    public void UpdateQuantity(int amount)
    {
        quantity = amount;
        structureQuantity.text = quantity.ToString();
        if (quantity <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void UpdateDescription()
    {
        if (quantity <= 0)
        {
            return;
        }
        buildQueueItemDescription.UpdateDescription(builtStructure.name, builtStructure.description);
    }

    private void ClearDescription()
    {
        buildQueueItemDescription.ResetToDefaultDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateDescription();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ClearDescription();
    }
}
