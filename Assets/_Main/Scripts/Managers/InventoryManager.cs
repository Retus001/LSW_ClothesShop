using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, SO_ClothingItem> clothingItems = new Dictionary<string, SO_ClothingItem>();

    public void AddClothingItem(SO_ClothingItem _item)
    {
        clothingItems.Add(_item.itemName, _item);
    }

    public void RemoveClothingItem(string _itemName)
    {
        clothingItems.Remove(_itemName);
    }
}
