using UnityEngine;

public class TabButton : MonoBehaviour
{
    public Transform tab;
    public void View()
    {
        tab.SetAsLastSibling();

    }
}
