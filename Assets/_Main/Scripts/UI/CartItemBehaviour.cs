using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartItemBehaviour : MonoBehaviour
{
    public TextMeshProUGUI m_itemName_TMP;
    public TextMeshProUGUI m_itemCost_TMP;
    public Image m_itemIcon;
    public SO_ClothingItem itemSO;

    public void SetupCartItem(SO_ClothingItem _item)
    {
        itemSO = _item;
        m_itemName_TMP.text = _item.itemName;
        m_itemCost_TMP.text = _item.itemCost.ToString("C");
        m_itemIcon.sprite = _item.itemIcon;
    }

    public void RemoveItemFromCart()
    {
        InventoryManager.Instance.RemoveClothingItemFromCart(itemSO.itemName);
    }
}
