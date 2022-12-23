using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationItemBehaviour : MonoBehaviour
{
    public Image m_itemIcon;
    public GameObject m_itemSelectedGraphic;
    public bool m_itemSelected;

    public SO_ClothingItem itemSO;

    public void SetupCustomizationItem(SO_ClothingItem _item)
    {
        itemSO = _item;
        m_itemIcon.sprite = _item.itemIcon;
        m_itemSelected = InventoryManager.Instance.m_equippedItems.ContainsValue(_item);
        m_itemSelectedGraphic.SetActive(m_itemSelected);
    }

    public void Interact()
    {
        if (m_itemSelected)
            InventoryManager.Instance.RemoveEquippedItem(itemSO);
        else
            InventoryManager.Instance.EquipItem(itemSO);
    }
}
