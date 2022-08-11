using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public DamageController DamageController;
    [HideInInspector] public Inventory Inventory;
    public InteractableItemsScriptableObject InteractableItemsScriptableObject;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Inventory = GetComponent<Inventory>();
    }
}
