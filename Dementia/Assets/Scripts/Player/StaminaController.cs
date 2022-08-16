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
    public StaminaBar staminaBar;
    private float _stamina;
    private float _staminaTimeOut = 3;
    private float _staminaRegenStartTime;
    private int _counter = 0;
    private float _staminaModeTimer = 20;

    private void Start()
    {
        _stamina = maxStamina;
    }
    
    private void FixedUpdate()
    {
        if (isInStaminaMode)
        {
            _staminaModeTimer -= Time.fixedDeltaTime;
            _stamina = maxStamina;
            staminaBar.slider.value = maxStamina;
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
                staminaBar.FadeOut();
                return;
            }
            _staminaRegenStartTime += Time.fixedDeltaTime;
            // _counter++;
            if (_staminaRegenStartTime >= _staminaTimeOut)
            {
                // _counter = 0;
                _stamina = _stamina < maxStamina ? _stamina + .1f : maxStamina;
                staminaBar.slider.value = _stamina;
                Debug.Log(_stamina);
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
        staminaBar.slider.value = _stamina;
        staminaBar.FadeIn();
        Debug.Log(_stamina);
    }

    public void StaminaMode()
    {
        isInStaminaMode = true;
        _stamina = maxStamina;
    }
    
}
