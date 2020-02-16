using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuiltStructureButton : MonoBehaviour
{
    public BuildQueueItemDescription buildQueueItemDescription;

    //the class of building that was built here
    public TextMeshProUGUI structureName;
    public TextMeshProUGUI structureQuantity;
    public BuiltStructure builtStructure;
    public int quantity = 0;
    public int structureClassIndex = 0;


    private void Start()
    {
        // StartCoroutine(Later());
        builtStructure = Master.instance.variety.builtStructures[structureClassIndex];
        structureName.text = builtStructure.name;
        structureQuantity.text = quantity.ToString();
    }

    /*
    IEnumerator Later(float time = 0.1f)
    {
        //yield return new WaitForSeconds(time);
       
    }
    */
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

    public void UpdateDescription()
    {
        if(quantity <= 0)
        {
            return;
        }
        buildQueueItemDescription.UpdateDescription(builtStructure.name, builtStructure.description);
    }
    

}
