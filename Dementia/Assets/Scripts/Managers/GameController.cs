using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Places
{
    HallwayFirstFloor,
    KitchenFirstFloor,
    DiningRoomFirstFloor,
    BathRoomFirstFloor,
    Park,
    PatientRoomFirstFloor
}

public class GameController : MonoBehaviour
{
    public DamageController DamageController;
    public StaminaController StaminaController;
    public PlayerController PlayerController;
    public InteractableItemsScriptableObject InteractableItemsScriptableObject;
    [HideInInspector] public Inventory Inventory;
    [HideInInspector] public FlashlightController FlashlightController;
    [HideInInspector] public LightsController LightsController;
    [HideInInspector] public JumpScareController JumpScareController;
    [HideInInspector] public Transform PlayerTransform;
    [HideInInspector] public bool KeysDisabled;

    [HideInInspector]
    public bool isInInventory;

    public static GameController instance;
    private GameManager _gameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        _gameManager = GameManager.instance;
        SavedData savedData = _gameManager.playerPrefsManager.LoadGame();
        PlayerController.transform.position = savedData.playerTransform.position;
        PlayerController.transform.rotation = savedData.playerTransform.rotation;
        Inventory = GetComponent<Inventory>();
        FlashlightController = GetComponent<FlashlightController>();
        LightsController = GetComponent<LightsController>();
        JumpScareController = GetComponent<JumpScareController>();
        PlayerTransform = PlayerController.transform;
        isInInventory = false;
        Time.timeScale = 1;
        KeysDisabled = false;
        LightsController.TurnAllLightsOnOrOff(_gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.LightsEnabled, true));
        
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public Transform GetPlayerTransform()
    {
        PlayerTransform = PlayerController.transform;
        return PlayerTransform;
    }
    
    public void SetPlayerTransform(Transform transform)
    {
        PlayerController.transform.position = transform.position;
        PlayerController.transform.rotation = transform.rotation;
    }

    public void DisableAllKeys()
    {
        KeysDisabled = true;
    }
    
    public void EnableAllKeys()
    {
        KeysDisabled = false;
    }
}
