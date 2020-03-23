using UnityEngine;
using UnityEngine.UI;

public class PlanetOverviewDescription : MonoBehaviour
{
    public RawImage image;
    public BuildQueueItemDescription buildQueueItemDescription;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateDescription(Planet planet)
    {
        image.texture = planet.planetTexture.texture2D;
        buildQueueItemDescription.SetDefaultDescription(planet.name, "description");
        buildQueueItemDescription.ResetToDefaultDescription();
    }
}
