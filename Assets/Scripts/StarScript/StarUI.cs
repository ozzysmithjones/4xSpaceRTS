using UnityEngine;

public class StarUI : MonoBehaviour
{

    public Animator factionBorderAnimator;
    public SpriteRenderer factionBorder;
    public StarConnections starConnections;
    public GameObject IconHandler;

    private CircleCollider2D uiHitCollider;

    // private StarFogOfWar starFogOfWar;

    // Start is called before the first frame update
    void Awake()
    {
        uiHitCollider = GetComponent<CircleCollider2D>();
        factionBorderAnimator = factionBorder.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialise()
    {
        starConnections = GetComponent<StarConnections>();
        //starFogOfWar = GetComponent<StarFogOfWar>();
    }

    public void SetUIVisibility(bool visibility)
    {
        uiHitCollider.enabled = visibility;
        factionBorderAnimator.SetBool("growing", visibility);
        IconHandler.SetActive(visibility);

    }

    public void SetUIColor(Color color)
    {
        factionBorder.color = color;
        starConnections.ChangeColor(color);
    }

}
