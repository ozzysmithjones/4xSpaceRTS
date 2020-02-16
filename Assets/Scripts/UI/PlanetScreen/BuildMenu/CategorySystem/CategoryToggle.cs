using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryToggle : MonoBehaviour
{
    public BuildQueueItem.Category category;
    public CategoryOverview categoryOverview;
    private Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCategory()
    {
        if (toggle.isOn)
        {
            print("is being toggled");
            categoryOverview.UpdateCategory(category, true);
        }
    }
}
