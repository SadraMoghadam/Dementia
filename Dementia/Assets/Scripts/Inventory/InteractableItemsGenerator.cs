using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableItemsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject itemsPlacementContainer;
    private List<Transform> _itemsPlaces;
    private GameController _gameController;
    private GameManager _gameManager;

    private void Start()
    {
        _gameController = GameController.instance;
        _gameManager = GameManager.instance;
        InstantiateInteractableItems();
    }

    private void InstantiateInteractableItems()
    {
        _itemsPlaces = itemsPlacementContainer.GetComponentsInChildren<Transform>().ToList();
        _itemsPlaces.RemoveAt(0);
        for (int i = 0; i < _itemsPlaces.Count; i++)
        {
            if (_itemsPlaces[i].GetComponent<InteractableItemInfo>() != null)
            {
                _itemsPlaces.RemoveAt(i);
            }
        }
        List<int> destroyedItemsId = _gameManager.playerPrefsManager.GetDestroyedInteractableItemsId();
        for (int i = 0; i < _itemsPlaces.Count; i++)
        {
            if ((destroyedItemsId != null && destroyedItemsId.Contains(i)))
                continue;
            InteractableItemType type = InteractableItemType.Battery;
            if (_itemsPlaces[i].gameObject.CompareTag(InteractableItemType.Battery.ToString()))
            {
                type = InteractableItemType.Battery;
            }
            else if (_itemsPlaces[i].gameObject.CompareTag(InteractableItemType.Key.ToString()))
            {
                type = InteractableItemType.Key;
            }
            else if (_itemsPlaces[i].gameObject.CompareTag(InteractableItemType.MedKit.ToString()))
            {
                type = InteractableItemType.MedKit;
            }
            else if (_itemsPlaces[i].gameObject.CompareTag(InteractableItemType.Flashlight.ToString()))
            {
                type = InteractableItemType.Flashlight;
            }
            else if (_itemsPlaces[i].gameObject.CompareTag(InteractableItemType.Pills.ToString()))
            {
                type = InteractableItemType.Pills;
            }
            else
            {
                break;
            }
            GameObject InstantiatedGO;
            int id = i;
            if (_itemsPlaces[i].childCount > 0)
            {
                InstantiatedGO = _itemsPlaces[i].GetComponentInChildren<InteractableItemInfo>().gameObject;
            }
            else
            {
                InstantiatedGO = Instantiate(_gameController.InteractableItemsScriptableObject.InteractableItems[(int)type].prefab, _itemsPlaces[i]);   
            }
            InstantiatedGO.GetComponent<InteractableItemInfo>().itemInfo.id = id;
            InstantiatedGO.gameObject.name = type.ToString();
        }
    }
}
