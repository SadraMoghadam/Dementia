using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[Serializable]
public class ItemInfo
{
    public int id;
    public InteractableItemType type;
}

public class InteractableItemInfo : MonoBehaviour
{
    public ItemInfo itemInfo;
}
