using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildOptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool Initialised = false;
    public BuildQueueItem.Category category = BuildQueueItem.Category.Economy;

    public BuildMenu buildMenu;
    private BuildQueueItem buildQueueItem;
    public TextMeshProUGUI itemName;
    public TMP_InputField quantity;
    public TextMeshProUGUI price;


    public void Initialise(BuildQueueItem buildQueueItem)
    {
        Initialised = true;
        gameObject.SetActive(true);
        this.buildQueueItem = buildQueueItem;

        category = buildQueueItem.category;
        itemName.text = buildQueueItem.name;
        price.text = "MAT: " + (buildQueueItem.buildCost * int.Parse(quantity.text));
        
    }

    public void Remove()
    {
        Initialised = false;
        gameObject.SetActive(false);
    }
    public void UpdatePrice()
    {
        price.text = "MAT: " + (buildQueueItem.buildCost * int.Parse(quantity.text));
    }

    public void Build()
    {
        buildMenu.Build(buildQueueItem, int.Parse(quantity.text));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buildMenu.BuildOptionDescription.text = buildQueueItem.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buildMenu.BuildOptionDescription.text = ".";
    }
}
