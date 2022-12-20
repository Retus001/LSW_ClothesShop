using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[Serializable]
public class ClothingTypeData
{
    public ClothingType m_clothingType;
    public Sprite m_clothingIconSprite;
}

public class UIManager : Singleton<UIManager>
{
    // Data
    public List<ClothingTypeData> m_clothingData = new List<ClothingTypeData>();

    // References
    public Sprite m_missingSprite;
    public TextMeshProUGUI m_priceTag_TMP;

    public GameObject m_storageWindow;
    public Transform m_clothingItemsContainer;
    public GameObject m_customizationWindow;
    public GameObject m_inventoryWindow;
    public GameObject m_priceTagWindow;

    public Button closeStorageWindow_btn;
    public Button closeCustomizationWindow_btn;
    public Button closeInventoryWindow_btn;

    // Prefabs
    public GameObject clothingPreview_pfb;

    [HideInInspector]
    public bool hoverable = false;
    private bool pointerHovered = false;

    void Start()
    {
        m_storageWindow.SetActive(false);
        m_customizationWindow.SetActive(false);
        m_inventoryWindow.SetActive(false);
        m_priceTagWindow.SetActive(false);
    }

    public void OpenStorageWindow(ClothingStorage _storage)
    {
        m_storageWindow.SetActive(true);
        hoverable = true;

        // Setup close window button
        closeStorageWindow_btn.onClick.RemoveAllListeners();
        closeStorageWindow_btn.onClick.AddListener(() => { CloseWindow(m_storageWindow); _storage.CloseStorage(); });

        // Instantiate storage UI items
        foreach(Transform child in m_clothingItemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(SO_ClothingItem _item in _storage.m_clothingItems)
        {
            ClothingPreviewBehaviour clothingItem = Instantiate(clothingPreview_pfb, m_clothingItemsContainer).GetComponent<ClothingPreviewBehaviour>();
            clothingItem.SetupClothingItem(_item.itemPreview, _item.itemCost, _item.itemName);
        }
    }

    private void CloseWindow(GameObject _window)
    {
        _window.SetActive(false);
        hoverable = false;
    }

    public Sprite GetClothingTypeIcon(ClothingType _type)
    {
        foreach (ClothingTypeData data in m_clothingData)
            if (data.m_clothingType == _type) return data.m_clothingIconSprite;

        return m_missingSprite;
    }

    public void ShowPriceTag(string _price)
    {
        m_priceTagWindow.SetActive(true);
        m_priceTag_TMP.text = _price;
    }

    public void HidePriceTag()
    {
        m_priceTagWindow.SetActive(false);
    }
}
