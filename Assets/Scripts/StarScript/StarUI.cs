using UnityEngine;

public class StarUI : MonoBehaviour
{

    public Animator factionBorderAnimator;
    public SpriteRenderer factionBorder;
    public StarConnections starConnections;
    public GameObject IconHandler;

    // private StarFogOfWar starFogOfWar;

    // Start is called before the first frame update
    void Start()
    {

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
        if (factionBorderAnimator == null)
        {
            //the sprite renderer somehow isnt null but the animator is  *shrug*.

            factionBorderAnimator = factionBorder.GetComponent<Animator>();
        }


        factionBorderAnimator.SetBool("growing", visibility);

        IconHandler.SetActive(visibility);

    }

    public void SetUIColor(Color color)
    {
        factionBorder.color = color;
        starConnections.ChangeColor(color);
    }

}
