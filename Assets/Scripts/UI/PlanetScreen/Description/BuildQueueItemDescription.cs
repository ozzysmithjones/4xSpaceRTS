using TMPro;
using UnityEngine;

public class BuildQueueItemDescription : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateDescription(string header, string content)
    {
        title.text = header;
        description.text = content;

    }
}
