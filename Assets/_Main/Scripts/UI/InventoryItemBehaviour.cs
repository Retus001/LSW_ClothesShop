using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemBehaviour : MonoBehaviour
{
    public Image m_myItemType;
    public Image m_myItemIcon;
    public Button m_sellButton;
    public TextMeshProUGUI m_sellValue_TMP;
    public SO_ClothingItem itemSO;
    private float sellValue;

    public void SetupInventoryItem(SO_ClothingItem _item)
    {
        itemSO = _item;
        m_myItemIcon.sprite = _item.itemIcon;
        m_myItemType.sprite = UIManager.Instance.GetClothingTypeIcon(_item.itemType);
    }

    public void EnableSelling(float _buyRate)
    {
        // Disable interactable if currently equipped
        if (InventoryManager.Instance.m_equippedItems.ContainsValue(itemSO))
        {
            m_sellButton.interactable = false;
            m_sellValue_TMP.text = "Equipped";
        } else
        {
            m_sellButton.interactable = true;
            m_sellValue_TMP.text = "Sell " + (itemSO.itemCost * _buyRate).ToString("C");
            sellValue = itemSO.itemCost * _buyRate;
        }
        m_sellButton.gameObject.SetActive(true);
    }

    public void DisableSelling()
    {
        m_sellButton.gameObject.SetActive(false);
    }

    public void SellItem()
    {
        if (InventoryManager.Instance.m_ownedClothingItems.ContainsValue(itemSO))
        {
            InventoryManager.Instance.RemoveClothingItem(itemSO.itemName);
            InventoryManager.Instance.ModifyMoney(sellValue);
        }
    }
}
