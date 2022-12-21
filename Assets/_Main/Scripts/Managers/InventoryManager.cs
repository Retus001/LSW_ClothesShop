using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InventoryManager : Singleton<InventoryManager>
{
    public Dictionary<string, SO_ClothingItem> ownedClothingItems = new Dictionary<string, SO_ClothingItem>();
    public Dictionary<string, SO_ClothingItem> cartClothingItems = new Dictionary<string, SO_ClothingItem>();
    public float money;

    public void AddClothingItem(SO_ClothingItem _item)
    {
        ownedClothingItems.Add(_item.itemName, _item);
    }

    public void RemoveClothingItem(string _itemName)
    {
        ownedClothingItems.Remove(_itemName);
    }

    public void AddClothingItemToCart(SO_ClothingItem _item)
    {
        cartClothingItems.Add(_item.itemName, _item);
    }

    public void RemoveClothingItemFromCart(string _itemName)
    {
        cartClothingItems.Remove(_itemName);
    }

    public float GetCartTotalCost()
    {
        float total = 0;
        foreach(SO_ClothingItem item in cartClothingItems.Values)
        {
            total += item.itemCost;
        }

        return total;
    }
}
