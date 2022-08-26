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
    [SerializeField] private GameObject deleteKeyDescription;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button closeButton;
    // [SerializeField] private GameObject itemsContainer;
    private int selectedItemId;
    
    private List<InventoryItem> _inventoryItemsObj;
    private List<InteractableItemType> _inventoryItemsType;
    private GameManager _gameManager;
    private GameController _gameController;
    private List<ItemInfo> _inventoryItemsInfo;
    private float _magnifyCoefficient = 1.2f;

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
        closeButton.onClick.AddListener(_gameController.HideCursor);
        deleteKeyDescription.SetActive(false);
        Color temp = itemImage.color;
        temp.a = 0;
        itemImage.color = temp;
        itemName.text = "";
        description.text = "";
        _inventoryItemsInfo = _gameManager.playerPrefsManager.GetInventoryInteractableItems();
        if(_inventoryItemsInfo == null)
            return;
        ClearInventoryItems();
        SetFlashlight();
        for (int i = 0; i < _inventoryItemsInfo.Count; i++)
        {
            var info = _inventoryItemsInfo[i];
            InteractableItems item = _gameController.Inventory.ConvertTypeToScriptableObject(info.type);
            _inventoryItemsObj[i + 1].id = info.id;
            _inventoryItemsObj[i + 1].type = info.type;
            _inventoryItemsObj[i + 1].itemImage.sprite = item.ItemScriptableObject.sprite;
            temp = _inventoryItemsObj[i + 1].itemImage.color;
            temp.a = 1;
            _inventoryItemsObj[i + 1].itemImage.color = temp;
            _inventoryItemsObj[i + 1].itemButton.onClick.AddListener(() => OnItemClick(item, info.id));
        }
    }

    private void ClearInventoryItems()
    {
        for (int i = 0; i < _inventoryItemsObj.Count; i++)
        {
            Color temp = _inventoryItemsObj[i].itemImage.color;
            temp.a = 0;
            _inventoryItemsObj[i].itemImage.color = temp;
        }
    }

    private void OnItemClick(InteractableItems item, int id)
    {
        selectedItemId = id;
        deleteKeyDescription.SetActive(true);
        itemImage.sprite = item.ItemScriptableObject.sprite;
        itemName.text = item.ItemScriptableObject.name;
        description.text = item.ItemScriptableObject.description;
        Color temp = itemImage.color;
        temp.a = 1;
        itemImage.color = temp;
        // for (int i = 0; i < _inventoryItemsObj.Count; i++)
        // {
        //     _inventoryItemsObj[i].gameObject.transform.localScale = Vector3.one;
        //     if (selectedCell == i)
        //     {
        //         _inventoryItemsObj[i].gameObject.transform.localScale = Vector3.one * _magnifyCoefficient;
        //         _inventoryItemsObj[i].itemButton.image.color = Color.gray;
        //     }
        // }
    }

    private void SetFlashlight()
    {
        if (_gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.HasFlashlight, false))
        {
            InteractableItems item = _gameController.Inventory.ConvertTypeToScriptableObject(InteractableItemType.Flashlight);
            _inventoryItemsObj[0].id = 0;
            _inventoryItemsObj[0].type = InteractableItemType.Flashlight;
            _inventoryItemsObj[0].itemImage.sprite = item.ItemScriptableObject.sprite;
            Color temp = _inventoryItemsObj[0].itemImage.color;
            temp.a = 1;
            _inventoryItemsObj[0].itemImage.color = temp;
            _inventoryItemsObj[0].itemButton.onClick.AddListener(() => OnItemClick(item, 0));   
        }
    }


    public void DeleteItem()
    {
        // for (int i = 0; i < _inventoryItemsInfo.Count; i++)
        // {
        //     if (_inventoryItemsInfo[i].id == selectedItemId)
        //     {
        //         _inventoryItemsInfo[i].deletedFromInventory = true;
        //         break;
        //     }
        // }
        _gameManager.playerPrefsManager.DeleteItemFromInventory(selectedItemId);
        Color temp = _inventoryItemsObj[_inventoryItemsInfo.Count].itemImage.color;
        temp.a = 0;
        _inventoryItemsObj[_inventoryItemsInfo.Count].itemImage.color = temp;
        SetInventory();
    }
    
}
