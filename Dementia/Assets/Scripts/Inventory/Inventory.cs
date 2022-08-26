using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InteractableItems> inventoryItems;
    public int inventorySpace = 6;
    private InventoryPanel _inventoryPanel;
    private GameManager _gameManager;
    private GameController _gameController;


    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameController = GameController.instance;
        _inventoryPanel = UIController.instance.inventoryPanel;
        InitItemsCount();
    }

    
    
    public void InitItemsCount()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].count = _gameManager.playerPrefsManager.GetInteractableItemCount(inventoryItems[i].ItemScriptableObject.type);
        }
    }

    public void AddItem(InteractableItemType type)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].ItemScriptableObject.type == type)
            {
                if (type == InteractableItemType.Flashlight)
                {
                    _gameManager.playerPrefsManager.SetBool(PlayerPrefsKeys.HasFlashlight, true);
                }
                else
                {
                    inventoryItems[i].count++;
                    _gameManager.playerPrefsManager.SetInteractableItem(inventoryItems[i]);
                    Debug.Log("Added " + type.ToString());   
                }
                break;
            }
        }
    }
    
    public void DeleteItem(InteractableItemType type)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].ItemScriptableObject.type == type)
            {
                if(inventoryItems[i].count != 0)
                    inventoryItems[i].count--;
                break;
            }
        }
    }

    public InteractableItems ConvertTypeToScriptableObject(InteractableItemType type)
    {
        InteractableItems item = new InteractableItems();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].ItemScriptableObject.type == type)
            {
                item = inventoryItems[i];
                break;
            }
        }

        return item;
    }

    public void OpenInventory()
    {
        _gameController.ShowCursor();
        _inventoryPanel.gameObject.SetActive(true);
    }

    public void CloseInventory()
    {
        _gameController.HideCursor();
        _inventoryPanel.gameObject.SetActive(false);
    }

    public InteractableItems GetItemInfo(InteractableItemType type)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].ItemScriptableObject.type == type)
            {
                return inventoryItems[i];
            }
        }

        return inventoryItems[0];
    }
    
    public void UpdateUsedItemOnPlayerPrefs(InteractableItemType type)
    {
        int itemCount = _gameManager.playerPrefsManager.GetInteractableItemCount(type);
        _gameManager.playerPrefsManager.SetInteractableItem(type, --itemCount);
    }
    
}
