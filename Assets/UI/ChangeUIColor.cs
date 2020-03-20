using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUIColor : MonoBehaviour
{
    public Image image;
    public bool isNewColor = false;
    public Color originalColor;
    public Color newColor;

    private void Start()
    {
        if (!isNewColor)
        {
            originalColor = image.color;
        }
        else
        {
            newColor = image.color;
        }
    }

    public void SwapColor()
    {
        isNewColor = !isNewColor;
        if (isNewColor)
        {
            image.color = newColor;
        }
        else
        {
            image.color = originalColor;
        }
    }
}
