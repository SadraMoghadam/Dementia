using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlight;
    private float _battery;
    private GameManager _gameManager;
    private FlashlightPanel _flashlightPanel;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _flashlightPanel = UIController.instance.flashlightPanel;
        _battery = _gameManager.playerPrefsManager.GetFloat(PlayerPrefsKeys.BatteryAmount, 0);
        _flashlightPanel.batterySlider.value = _battery;
    }

    public void ReduceBatteryOverTime(float amount)
    {
        _battery -= amount;
        if (Mathf.Round(_battery) == 10)
        {
            GameController.instance.QuestAndHintController.ShowHint(4);
        }
        if (_battery < 0)
        {
            _battery = 0;
            ChangeFlashlightState(false);
        }

        _flashlightPanel.batterySlider.value = _battery;
        _gameManager.playerPrefsManager.SetFloat(PlayerPrefsKeys.BatteryAmount, _battery);
    }

    public void ReloadBattery()
    {
        if (_battery != 100)
        {
            _battery = 100;
            _flashlightPanel.batterySlider.value = _battery;
            _gameManager.playerPrefsManager.SetFloat(PlayerPrefsKeys.BatteryAmount, 100);
        }
    }

    
    public void ChangeFlashlightState(bool flashlightOn)
    {
        _flashlightPanel.flashlightImage.sprite = flashlightOn ? _flashlightPanel.flashlightOn : _flashlightPanel.flashlightOff;
        flashlight.SetActive(flashlightOn);
    }

    public float GetBatteryAmount()
    {
        return _battery;
    }
    
}
