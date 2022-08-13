using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public DamageController DamageController;
    [HideInInspector] public Inventory Inventory;
    public InteractableItemsScriptableObject InteractableItemsScriptableObject;

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
        isInInventory = false;
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
