using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerPrefsKeys
{
    HasFlashlight,
    DestroyedInteractableItems,
    InventoryInteractableItemsCount,
    BatteryAmount
}

public class PlayerPrefsManager : MonoBehaviour
{
    public void SetBool(PlayerPrefsKeys playerPrefsKeys, bool value)
    {
        PlayerPrefs.SetInt(playerPrefsKeys.ToString(), value ? 1 : 0);
    }
    
    public bool GetBool(PlayerPrefsKeys playerPrefsKeys, bool defaultValue = true)
    {
        int value = defaultValue ? 1 : 0;
        if (PlayerPrefs.HasKey(playerPrefsKeys.ToString()))
        {
            value = PlayerPrefs.GetInt(playerPrefsKeys.ToString());   
        }
        return value == 1 ? true : false;
    }
    
    public void SetFloat(PlayerPrefsKeys playerPrefsKeys, float value)
    {
        PlayerPrefs.SetFloat(playerPrefsKeys.ToString(), value);
    }
    
    public float GetFloat(PlayerPrefsKeys playerPrefsKeys, float defaultValue)
    {
        float value = defaultValue;
        if (PlayerPrefs.HasKey(playerPrefsKeys.ToString()))
        {
            value = PlayerPrefs.GetFloat(playerPrefsKeys.ToString());   
        }
        return value;
    }
    
    public void SetInt(PlayerPrefsKeys playerPrefsKeys, int value)
    {
        PlayerPrefs.SetInt(playerPrefsKeys.ToString(), value);
    }
    
    public int GetInt(PlayerPrefsKeys playerPrefsKeys, int defaultValue)
    {
        int value = defaultValue;
        if (PlayerPrefs.HasKey(playerPrefsKeys.ToString()))
        {
            value = PlayerPrefs.GetInt(playerPrefsKeys.ToString());   
        }
        return value;
    }
    
    public void SetString(PlayerPrefsKeys playerPrefsKeys, string value)
    {
        PlayerPrefs.SetString(playerPrefsKeys.ToString(), value);
    }
    
    public string GetString(PlayerPrefsKeys playerPrefsKeys, string defaultValue)
    {
        string value = defaultValue;
        if (PlayerPrefs.HasKey(playerPrefsKeys.ToString()))
        {
            value = PlayerPrefs.GetString(playerPrefsKeys.ToString());   
        }
        return value;
    }

    public void SetInteractableItem(InteractableItems item)
    {
        SetInteractableItem(item.ItemScriptableObject.type, item.count);
        int currentInventoryItems = GetInt(PlayerPrefsKeys.InventoryInteractableItemsCount, 0);
        SetInt(PlayerPrefsKeys.InventoryInteractableItemsCount, ++currentInventoryItems);
    }
    
    public void SetInteractableItem(InteractableItemType type, int count)
    {
        PlayerPrefs.SetInt(type.ToString(), count);
    }

    public List<InteractableItemType> GetInventoryItemsType()
    {
        List<InteractableItemType> inventoryItems = new List<InteractableItemType>();
        for (int i = 0; i < Enum.GetValues(typeof(InteractableItemType)).Length; i++)
        {
            InteractableItemType type = (InteractableItemType)i;
            int numOfItems = PlayerPrefs.GetInt(type.ToString(), 0);
            if(numOfItems > 0)
                inventoryItems.Add(type);
        }
        return inventoryItems;
    }
    
    public int GetInteractableItemCount(InteractableItemType type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
            return 0;
        return PlayerPrefs.GetInt(type.ToString()); 
    }

    
    [Serializable]
    private struct InteractableItemsInfo
    {
        public List<ItemInfo> items;
    } 
    
    public void AddDestroyedInteractableItem(InteractableItemInfo itemInfo)
    {
        List<ItemInfo> itemsInfo = new List<ItemInfo>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.DestroyedInteractableItems.ToString()))
            itemsInfo = GetDestroyedInteractableItems();
        string itemsString = "";
        ItemInfo info = new ItemInfo();
        info.id = itemInfo.itemInfo.id;
        info.type = itemInfo.itemInfo.type;
        itemsInfo.Add(info);
        InteractableItemsInfo interactableItemsInfo = new InteractableItemsInfo();
        interactableItemsInfo.items = itemsInfo;
        itemsString = JsonUtility.ToJson(interactableItemsInfo);
        SetString(PlayerPrefsKeys.DestroyedInteractableItems, itemsString);
    }
    
    public List<ItemInfo> GetDestroyedInteractableItems()
    {
        string itemsString = "";
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.DestroyedInteractableItems.ToString()))
        {
            itemsString = GetString(PlayerPrefsKeys.DestroyedInteractableItems, "");
            InteractableItemsInfo interactableItemsInfo = JsonUtility.FromJson<InteractableItemsInfo>(itemsString);
            return interactableItemsInfo.items;
        }
        return null;
    }
    
    public List<int> GetDestroyedInteractableItemsId()
    {
        List<ItemInfo> infos = GetDestroyedInteractableItems();
        List<int> ids = new List<int>();
        if (infos == null)
            return null;
        
        for (int i = 0; i < infos.Count; i++)
        {
            ids.Add(infos[i].id);
        }

        return ids;
    }

    
    public List<ItemInfo> GetInventoryInteractableItems()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.DestroyedInteractableItems.ToString()))
        {
            List<ItemInfo> destroyedItemsInfo = GetDestroyedInteractableItems();
            List<ItemInfo> inventoryItemsInfo = new List<ItemInfo>();
            int lowerBoundLoop = destroyedItemsInfo.Count < 6 ? 0 : destroyedItemsInfo.Count - 6;
            int i = destroyedItemsInfo.Count - 1;
            while (inventoryItemsInfo.Count < 6)
            {
                if(i == -1)
                    break;
                if (destroyedItemsInfo[i].type == InteractableItemType.Flashlight || destroyedItemsInfo[i].deletedFromInventory)
                {
                    i--;
                    continue;
                }
                inventoryItemsInfo.Add(destroyedItemsInfo[i]);
                i--;
            }
            return inventoryItemsInfo;    
        }
        return null;
    }

    public void DeleteItemFromInventory(int id)
    {
        List<ItemInfo> itemsInfo = GetDestroyedInteractableItems();
        for (int i = 0; i < itemsInfo.Count; i++)
        {
            if (itemsInfo[i].id == id)
            {
                if (itemsInfo[i].type == InteractableItemType.Flashlight)
                {
                    return;
                }
                itemsInfo[i].deletedFromInventory = true;
                int inventoryItemsCount = GetInt(PlayerPrefsKeys.InventoryInteractableItemsCount, 0);
                SetInt(PlayerPrefsKeys.InventoryInteractableItemsCount, --inventoryItemsCount);
                int itemCount = GetInteractableItemCount(itemsInfo[i].type);
                SetInteractableItem(itemsInfo[i].type, --itemCount);
                GameController.instance.Inventory.DeleteItem(itemsInfo[i].type);
                break;
            }
        }

        InteractableItemsInfo interactableItemsInfo = new InteractableItemsInfo();
        interactableItemsInfo.items = itemsInfo;
        string itemsString = JsonUtility.ToJson(interactableItemsInfo);
        SetString(PlayerPrefsKeys.DestroyedInteractableItems, itemsString);
    }

    public List<int> GetItemsInInventoryIds(InteractableItemType type)
    {
        List<int> ids = new List<int>();
        var inventoryItems = GetInventoryInteractableItems();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].type == type)
            {
                ids.Add(inventoryItems[i].id);
            }
        }

        return ids;
    }
    
    
    // [Serializable]
    // public struct CurrentInteractableItems
    // {
    //     public List<InteractableItems> items;
    // } 
    //
    // public void AddInteractableItem(InteractableItems item)
    // {
    //     string itemsJson = "";
    //     List<InteractableItems> items = new List<InteractableItems>();
    //     CurrentInteractableItems currentInteractableItems = new CurrentInteractableItems();
    //     if (!PlayerPrefs.HasKey(PlayerPrefsKeys.InteractableItems.ToString()))
    //     {
    //         items.Add(item);
    //         currentInteractableItems.items = items;
    //         itemsJson = JsonUtility.ToJson(currentInteractableItems);
    //         SetString(PlayerPrefsKeys.InteractableItems, itemsJson);
    //     }
    //     else
    //     {
    //         items = GetInteractableItems();
    //         for (int i = 0; i < items.Count; i++)
    //         {
    //             if (items[i].item == item.item)
    //             {
    //                 items[i] = item;
    //                 currentInteractableItems.items = items;
    //                 itemsJson = JsonUtility.ToJson(currentInteractableItems);
    //                 SetString(PlayerPrefsKeys.InteractableItems, itemsJson);
    //                 return;
    //             }
    //         }
    //         items.Add(item);
    //         currentInteractableItems.items = items;
    //         itemsJson = JsonUtility.ToJson(currentInteractableItems);
    //         SetString(PlayerPrefsKeys.InteractableItems, itemsJson);
    //     }
    //     return;
    // }
    //
    // public List<InteractableItems> GetInteractableItems()
    // {
    //     CurrentInteractableItems currentInteractableItems = new CurrentInteractableItems();
    //     if (PlayerPrefs.HasKey(PlayerPrefsKeys.InteractableItems.ToString()))
    //     {
    //         string jsonLevels = PlayerPrefs.GetString(PlayerPrefsKeys.InteractableItems.ToString());
    //         currentInteractableItems = JsonUtility.FromJson<CurrentInteractableItems>(jsonLevels);
    //     }
    //     else
    //     {
    //         return new List<InteractableItems>();
    //     }
    //     return currentInteractableItems.items;
    // }

}
