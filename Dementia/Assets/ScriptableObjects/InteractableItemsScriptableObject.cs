using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableItems", menuName = "Items/InteractableItems")]
public class InteractableItemsScriptableObject : ScriptableObject
{
    public List<InteractableItem> InteractableItems;
}
