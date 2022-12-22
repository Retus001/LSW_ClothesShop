using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemBehaviour : MonoBehaviour
{
    public Image m_myItemType;
    public Image m_myItemIcon;

    public void SetupInventoryItem(Sprite _itemIcon, Sprite _itemTypeSprite)
    {
        m_myItemIcon.sprite = _itemIcon;
        m_myItemType.sprite = _itemTypeSprite;
    }
}
