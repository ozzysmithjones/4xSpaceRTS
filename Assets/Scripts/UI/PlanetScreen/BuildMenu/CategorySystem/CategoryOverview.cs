using UnityEngine;

public class CategoryOverview : MonoBehaviour
{
    public BuildMenu buildMenu;
    // Start is called before the first frame update

    private void OnEnable()
    {
       // UpdateCategory(BuildQueueItem.Category.Economy, true);
    }

    public void UpdateCategory(BuildQueueItem.Category category, bool active)
    {
        for (int i = 0; i < buildMenu.buildOptions.Length; i++)
        {
            if (!buildMenu.buildOptions[i].Initialised)
            {
                continue;
            }

            if (buildMenu.buildOptions[i].category != category && active)
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
