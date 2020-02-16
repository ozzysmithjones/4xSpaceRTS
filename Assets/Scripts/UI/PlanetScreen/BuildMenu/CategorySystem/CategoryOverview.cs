using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryOverview : MonoBehaviour
{
    public BuildMenu buildMenu;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCategory(BuildQueueItem.Category.Economy, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCategory(BuildQueueItem.Category category, bool active)
    {
        for(int i = 0; i < buildMenu.buildOptions.Length; i++)
        {
            if(buildMenu.buildOptions[i].category != category && active)
            {
                //print("deactivation happens");
                buildMenu.buildOptions[i].gameObject.SetActive(false);
            }
            else
            {
                //print("doesn't happen");
                buildMenu.buildOptions[i].gameObject.SetActive(true);
            }
        }
    }
}
