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
    

    [HideInInspector]
    public bool isInInventory;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Inventory = GetComponent<Inventory>();
        FlashlightController = GetComponent<FlashlightController>();
        LightsController = GetComponent<LightsController>();
        isInInventory = false;
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

}
