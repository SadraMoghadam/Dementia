using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaController : MonoBehaviour
{
    [HideInInspector] public bool isInStaminaMode;
    [HideInInspector] public float maxStamina = 100;
    [SerializeField] private float staminaModeTime = 20;
    [SerializeField] private float regenerationDelay = 0;
    private StaminaBar _staminaBar;
    private float _stamina;
    private float _staminaTimeOut = 3;
    private float _staminaRegenStartTime;
    private int _counter = 0;
    private float _staminaModeTimer = 20;
    private UIController _uiController;

    private void Start()
    {
        _stamina = maxStamina;
        _uiController = UIController.instance;
        _staminaBar = _uiController.staminaBar;
    }
    
    private void FixedUpdate()
    {
        if(!_uiController.canvas.activeSelf)
            return;
        if (isInStaminaMode)
        {
            _staminaModeTimer -= Time.fixedDeltaTime;
            _stamina = maxStamina;
            _staminaBar.slider.value = maxStamina;
            _staminaBar.staminaText.text = ((int)_stamina).ToString();
            if (_staminaModeTimer <= 0)
            {
                _staminaModeTimer = staminaModeTime;
                isInStaminaMode = false;
            }
        }
        else
        {
            if (_stamina >= maxStamina)
            {
                _stamina = maxStamina;
                // _staminaBar.FadeOut();
                return;
            }
            _staminaRegenStartTime += Time.fixedDeltaTime;
            // _counter++;
            if (_staminaRegenStartTime >= _staminaTimeOut)
            {
                // _counter = 0;
                _stamina = _stamina < maxStamina ? _stamina + .2f : maxStamina;
                _staminaBar.slider.value = _stamina;
                _staminaBar.staminaText.text = ((int)_stamina).ToString();
                
            }    
        }
        
    }

    public void ReduceStaminaOverTime(float amount)
    {
        _staminaRegenStartTime = 0;
        _stamina -= amount;
        if (_stamina < 0)
        {
            _stamina = 0;
        }
        _staminaBar.slider.value = _stamina;
        _staminaBar.staminaText.text = ((int)_stamina).ToString();
        // _staminaBar.FadeIn();
        Debug.Log(_stamina);
    }

    public void StaminaMode()
    {
        isInStaminaMode = true;
        _stamina = maxStamina;
    }
    
    
    public float GetStamina()
    {
        return _stamina;
    }
    
}
