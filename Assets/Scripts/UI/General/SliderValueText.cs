using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SliderValueText : MonoBehaviour
{
    public Slider slider;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateValue(int multiplier = 100)
    {
        text.text = Mathf.FloorToInt(slider.value * 100).ToString() + "%";
    }
}
