using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InventoryManager : Singleton<InventoryManager>
{
    public Dictionary<string, SO_ClothingItem> m_ownedClothingItems = new Dictionary<string, SO_ClothingItem>();
    public Dictionary<string, SO_ClothingItem> m_cartClothingItems = new Dictionary<string, SO_ClothingItem>();
    public float m_startingBalance;
    [HideInInspector]
    public float m_balance;

    // Money updated event
    public delegate void UpdateMoney();
    public static event UpdateMoney OnUpdateMoney;

    // Update owned items event
    public delegate void UpdateInventory();
    public static event UpdateInventory OnUpdateInventory;

    // Update items cart
    public delegate void UpdateCart();
    public static event UpdateCart OnUpdateCart;

    private void Start()
    {
        ModifyMoney(m_startingBalance);
        ResetCart();
        ResetOwnedClothing();
    }

    public void AddClothingItem(SO_ClothingItem _item)
    {
        m_ownedClothingItems.Add(_item.itemName, _item);
        if (OnUpdateInventory != null) OnUpdateInventory();
    }

    public void RemoveClothingItem(string _itemName)
    {
        m_ownedClothingItems.Remove(_itemName);
        if (OnUpdateInventory != null) OnUpdateInventory();
    }

    public void ResetOwnedClothing()
    {
        m_ownedClothingItems.Clear();
        if (OnUpdateInventory != null) OnUpdateInventory();
    }

    public void AddClothingItemToCart(SO_ClothingItem _item)
    {
        m_cartClothingItems.Add(_item.itemName, _item);
        if (OnUpdateCart != null) OnUpdateCart();
    }

    public void RemoveClothingItemFromCart(string _itemName)
    {
        m_cartClothingItems.Remove(_itemName);
        if (OnUpdateCart != null) OnUpdateCart();
    }

    public void ResetCart()
    {
        m_cartClothingItems.Clear();
        if (OnUpdateCart != null) OnUpdateCart();
    }

    public void ModifyMoney(float _amount)
    {
        m_balance += _amount;
        if (OnUpdateMoney != null) OnUpdateMoney();
    }

    public void SetMoney(float _amount)
    {
        m_balance = _amount;
        if (OnUpdateMoney != null) OnUpdateMoney();
    }

    public float GetCartTotalCost()
    {
        float total = 0;
        foreach(SO_ClothingItem item in m_cartClothingItems.Values)
        {
            total += item.itemCost;
        }

        return total;
    }
}
