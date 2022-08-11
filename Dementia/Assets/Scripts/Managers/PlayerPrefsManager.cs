using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerPrefsKeys
{
    HasFlashlight,
    DestroyedInteractableItems
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
        PlayerPrefs.SetInt(item.ItemScriptableObject.type.ToString(), item.count);
    }
    
    public int GetInteractableItemCount(InteractableItemType type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
            return 0;
        return PlayerPrefs.GetInt(type.ToString()); 
    }

    public void AddDestroyedInteractableItemId(int itemId)
    {
        List<int> itemsId = new List<int>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.DestroyedInteractableItems.ToString()))
            itemsId = GetDestroyedInteractableItemsId();
        string itemsString = "";
        itemsId.Add(itemId);
        itemsString = String.Join(',', itemsId);
        SetString(PlayerPrefsKeys.DestroyedInteractableItems, itemsString);
    }
    
    public List<int> GetDestroyedInteractableItemsId()
    {
        string itemsString = "";
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.DestroyedInteractableItems.ToString()))
        {
            itemsString = GetString(PlayerPrefsKeys.DestroyedInteractableItems, "");
            List<string> idsString = itemsString.Split( new [] {','} ).ToList();
            List<int> ids = new List<int>();
            for (int i = 0; i < idsString.Count; i++)
            {
                ids.Add(Int16.Parse(idsString[i])); 
            }
            return ids;
        }
        return null;
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
