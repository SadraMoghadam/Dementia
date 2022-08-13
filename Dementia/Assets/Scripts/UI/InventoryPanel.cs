using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject itemsContainer;

    private List<InventoryItem> _inventoryItemsObj;
    private List<InteractableItemType> _inventoryItemsType;
    private GameManager _gameManager;
    private GameController _gameController;
    private List<ItemInfo> _inventoryItemsInfo;

    private void OnEnable()
    {
        _inventoryItemsObj = GetComponentsInChildren<InventoryItem>().ToList();
        _gameManager = GameManager.instance;
        _gameController = GameController.instance;
        _inventoryItemsInfo = new List<ItemInfo>();
        SetInventory();
    }

    private void SetInventory()
    {
        _inventoryItemsInfo = _gameManager.playerPrefsManager.GetInventoryInteractableItems();
        if(_inventoryItemsInfo == null)
            return;
        for (int i = 0; i < _inventoryItemsInfo.Count; i++)
        {
            var info = _inventoryItemsInfo[i];
            InteractableItems item = _gameController.Inventory.ConvertTypeToScriptableObject(info.type);
            _inventoryItemsObj[i].id = info.id;
            _inventoryItemsObj[i].type = info.type;
            _inventoryItemsObj[i].itemImage.sprite = item.ItemScriptableObject.sprite;
            Color temp = _inventoryItemsObj[i].itemImage.color;
            temp.a = 1;
            _inventoryItemsObj[i].itemImage.color = temp;
        }
    }
    
}
