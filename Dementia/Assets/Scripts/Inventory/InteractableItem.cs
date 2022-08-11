using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableItemType
{
    Key,
    MedKit,
    Battery,
    Flashlight
}


[CreateAssetMenu(fileName = "InteractableItems", menuName = "Items/InteractableItem")]
public class InteractableItem : ScriptableObject
{
    public string name;
    public GameObject prefab;
    public InteractableItemType type;
    public float effect;
}
