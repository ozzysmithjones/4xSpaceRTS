using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    public GameObject menu;
    public bool active = false;
    private void Awake()
    {
        menu.SetActive(false);
    }
    public void Toggle()
    {
        active = !active;
        menu.SetActive(active);
    }
}
