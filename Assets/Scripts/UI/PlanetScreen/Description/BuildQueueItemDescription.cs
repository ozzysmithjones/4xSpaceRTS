using TMPro;
using UnityEngine;

public class BuildQueueItemDescription : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private string defaultHeader;
    private string defaultDescription;

    public void SetDefaultDescription(string header, string description)
    {
        defaultHeader = header;
        defaultDescription = description;
    }


    public void UpdateDescription(string header, string content)
    {
        title.text = header;
        description.text = content;

    }

    public void ResetToDefaultDescription()
    {
        title.text = defaultHeader;
        description.text = defaultDescription;
    }
}
