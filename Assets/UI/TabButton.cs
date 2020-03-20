using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public Transform tab;
    public void View()
    {
        tab.SetAsLastSibling();

    }
}
