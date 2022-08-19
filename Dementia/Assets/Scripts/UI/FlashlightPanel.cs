using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightPanel : MonoBehaviour
{
    public Slider batterySlider;
    public Image flashlightImage;
    public Sprite flashlightOn;
    public Sprite flashlightOff;
    [HideInInspector] public float maxBattery = 100;

    private void Start()
    {
        flashlightImage.sprite = flashlightOff;
        batterySlider.value = maxBattery;
    }


}
