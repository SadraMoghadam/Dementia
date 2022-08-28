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
    public InteractableItemsScriptableObject InteractableItemsScriptableObject;
    [HideInInspector] public Inventory Inventory;
    [HideInInspector] public FlashlightController FlashlightController;
    [HideInInspector] public LightsController LightsController;
    [HideInInspector] public JumpScareController JumpScareController;
    [HideInInspector] public Transform PlayerTransform;

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
        DamageController.transform.position = savedData.playerTransform.position;
        DamageController.transform.rotation = savedData.playerTransform.rotation;
        Inventory = GetComponent<Inventory>();
        FlashlightController = GetComponent<FlashlightController>();
        LightsController = GetComponent<LightsController>();
        JumpScareController = GetComponent<JumpScareController>();
        PlayerTransform = DamageController.transform;
        isInInventory = false;
        Time.timeScale = 1;
    }

    private void Start()
    {
        //StartCoroutine(LightsController.RandomFlickeryLightInPlace(Places.HallwayFirstFloor, 20));
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
        PlayerTransform = DamageController.transform;
        return PlayerTransform;
    }

}
