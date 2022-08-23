using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private FlashlightPanel flashlightPanel;
    private float _battery;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _battery = _gameManager.playerPrefsManager.GetFloat(PlayerPrefsKeys.BatteryAmount, 0);
        flashlightPanel.batterySlider.value = _battery;
    }

    public void ReduceBatteryOverTime(float amount)
    {
        _battery -= amount;
        if (_battery < 0)
        {
            _battery = 0;
            ChangeFlashlightState(false);
        }

        flashlightPanel.batterySlider.value = _battery;
        _gameManager.playerPrefsManager.SetFloat(PlayerPrefsKeys.BatteryAmount, _battery);
    }

    public void ReloadBattery()
    {
        if (_battery != 100)
        {
            _battery = 100;
            flashlightPanel.batterySlider.value = _battery;
            _gameManager.playerPrefsManager.SetFloat(PlayerPrefsKeys.BatteryAmount, 100);
        }
    }

    
    public void ChangeFlashlightState(bool flashlightOn)
    {
        flashlightPanel.flashlightImage.sprite = flashlightOn ? flashlightPanel.flashlightOn : flashlightPanel.flashlightOff;
    }

    public float GetBatteryAmount()
    {
        return _battery;
    }
    
}
