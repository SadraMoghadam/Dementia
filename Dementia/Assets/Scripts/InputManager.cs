using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControlls playerControlls;
    private static InputManager _instance;

    public static InputManager Instance
    {
        get => _instance;
        
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControlls = new PlayerControlls();
    }

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControlls.Player.Movement.ReadValue<Vector2>();
    }
    
    public Vector2 GetMouseDelta()
    {
        return playerControlls.Player.Look.ReadValue<Vector2>();
    }
}
