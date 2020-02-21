using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildOptionUI : MonoBehaviour
{
    public BuildQueueItem.Category category = BuildQueueItem.Category.Economy;

    public BuildMenu buildMenu;
    public bool isShip = false;
    public int classIndex = 0;
    private BuildQueueItem buildQueueItem;
    public TextMeshProUGUI itemName;
    public TMP_InputField quantity;
    public TextMeshProUGUI price;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isShip) {

            buildQueueItem = Master.instance.factions.factions[0].structureTypes[classIndex];

        }
        else
        {
            buildQueueItem = Master.instance.factions.factions[0].shipTypes[classIndex];
        }

        category = buildQueueItem.category;
        itemName.text = buildQueueItem.name;
        price.text = "MAT: " + buildQueueItem.buildCost.ToString();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build()
    {
        buildMenu.Build(buildQueueItem, int.Parse(quantity.text));
    }
}
