[System.Serializable]
public class Tool
{
    public bool active = false;
    public virtual void OnSelected()
    {

    }

    public virtual void OnDeselected()
    {

    }

    public virtual void OnInteractStar(Star star)
    {

    }

    public virtual void OnHoverStar(Star star)
    {

    }

    public virtual void OnInteractPlanet(Planet planet)
    {

    }

    public virtual void OnHoverPlanet(Planet planet)
    {

    }
}
