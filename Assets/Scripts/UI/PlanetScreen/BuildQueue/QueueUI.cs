using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueueUI : MonoBehaviour
{
    public PlanetOverview planetOverview;
    public int itemIndex = 0;
    public Slider progressBar;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI quantity;

    private Queue queue;

    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Remove);

        progressBar.normalizedValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProgress(float value)
    {
        progressBar.normalizedValue = value;
    }

    public float UpdateProgress()
    {
        progressBar.normalizedValue = (Time.time - queue.startTime) / queue.item.buildTime;
        quantity.text = queue.quantity.ToString();
        return progressBar.normalizedValue;
    }


    public void UpdateUI(Queue queue)
    {
        this.queue = queue;
        itemName.text = this.queue.item.name;
        quantity.text = this.queue.quantity.ToString();
    }
    public void UpdateUI(int index, float progressValue, string newItemName, string newQuantity)
    {
        itemIndex = index;
        progressBar.normalizedValue = progressValue;
        itemName.text = newItemName;
        quantity.text = newQuantity;
    }

    public void Remove()
    {
        progressBar.normalizedValue = 0f;
        itemIndex = planetOverview.planet.planetColony.buildQueue.IndexOf(queue);
        Debug.Log(itemIndex);
        planetOverview.planet.planetColony.RemoveFromBuildQueue(itemIndex, Input.GetKey(KeyCode.LeftShift));

    }
}
