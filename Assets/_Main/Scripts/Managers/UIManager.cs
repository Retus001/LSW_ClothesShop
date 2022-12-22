using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

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
    public TextMeshProUGUI m_priceTagItemName_TMP;

    public RectTransform m_storageWindow;
    public Transform m_clothingItemsContainer;
    public RectTransform m_customizationWindow;
    public RectTransform m_inventoryWindow;
    public RectTransform m_toggleInventoryArrow;
    public RectTransform m_cartWindow;
    public RectTransform m_toggleCartArrow;
    public RectTransform m_priceTagWindow;
    public RectTransform m_bankBalanceWindow;
    public RectTransform m_toggleBankArrow;
    public TextMeshProUGUI m_bankBalanceDisplay_TMP;
    public TextMeshProUGUI m_cartItemCount_TMP;
    public TextMeshProUGUI m_cartItemsTotal_TMP;
    public GameObject m_MainMenuWindow;
    public Canvas[] sidePanelCanvas;

    public Button closeStorageWindow_btn;

    // Prefabs
    public GameObject clothingPreview_pfb;

    private bool inventoryOpen = false;
    private bool cartOpen = false;
    private bool bankOpen = false;

    private List<GameObject> cartItemsDisplayed = new List<GameObject>();
    private List<GameObject> inventoryItemsDisplayed = new List<GameObject>();

    private void OnEnable()
    {
        InventoryManager.OnUpdateMoney += UpdateBankBalanceDisplay;
        InventoryManager.OnUpdateInventory += UpdateInventoryWindow;
        InventoryManager.OnUpdateCart += OpenCartWindow;
        InventoryManager.OnUpdateCart += UpdateCartWindow;
        InputManager.OnMainMenuPress += ToggleMainMenu;
    }

    private void OnDisable()
    {
        InventoryManager.OnUpdateMoney -= UpdateBankBalanceDisplay;
        InventoryManager.OnUpdateInventory -= UpdateInventoryWindow;
        InventoryManager.OnUpdateCart += OpenCartWindow;
        InventoryManager.OnUpdateCart -= UpdateCartWindow;
        InputManager.OnMainMenuPress -= ToggleMainMenu;
    }

    void Start()
    {
        m_storageWindow.gameObject.SetActive(false);
        m_customizationWindow.gameObject.SetActive(false);
        m_priceTagWindow.gameObject.SetActive(false);
        m_MainMenuWindow.SetActive(false);
        CloseInventoryWindow();
        CloseBankBalance();
        CloseCartWindow();
        
        foreach(Canvas canvas in sidePanelCanvas)
        {
            canvas.sortingOrder = 1;
        }
    }

    public void ToggleMainMenu()
    {
        m_MainMenuWindow.SetActive(!m_MainMenuWindow.activeSelf);
    }

    public void OpenStorageWindow(ClothingStorage _storage)
    {
        m_storageWindow.gameObject.SetActive(true);

        // Setup close window button
        closeStorageWindow_btn.onClick.RemoveAllListeners();
        closeStorageWindow_btn.onClick.AddListener(() => { CloseWindow(m_storageWindow.gameObject); _storage.CloseStorage(); });

        // Instantiate storage UI items
        foreach(Transform child in m_clothingItemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(SO_ClothingItem _item in _storage.m_clothingItems)
        {
            ClothingPreviewBehaviour clothingItem = Instantiate(clothingPreview_pfb, m_clothingItemsContainer).GetComponent<ClothingPreviewBehaviour>();
            clothingItem.SetupClothingItem(_item);
        }
    }

    public void PushSidePanelToFront(int _index)
    {
        for(int i = 0; i < sidePanelCanvas.Length; i++)
        {
            if (i == _index)
                sidePanelCanvas[i].sortingOrder = 2;
            else
                sidePanelCanvas[i].sortingOrder = 1;
        }
    }

    public void ToggleCartWindow()
    {
        if (cartOpen)
            CloseCartWindow();
        else
            OpenCartWindow();
    }

    public void OpenCartWindow()
    {
        cartOpen = true;
        m_cartWindow.gameObject.SetActive(true);
        m_cartWindow.DOAnchorPosX(0, 0.5f);
        m_toggleCartArrow.DOLocalRotate(new Vector3(0, 0, -180), 0.5f);
        PushSidePanelToFront(2);
    }

    public void UpdateCartWindow()
    {
        foreach(GameObject item in cartItemsDisplayed)
        {
            PoolManager.Instance.ResetObjInstance(item, PoolObjectType.CartItem);
        }
        cartItemsDisplayed.Clear();

        foreach(SO_ClothingItem item in InventoryManager.Instance.m_cartClothingItems.Values)
        {
            GameObject cartItem = PoolManager.Instance.GetPoolObject(PoolObjectType.CartItem);
            CartItemBehaviour itemBehaviour = cartItem.GetComponent<CartItemBehaviour>();
            itemBehaviour.SetupCartItem(item);
            cartItem.SetActive(true);
            cartItemsDisplayed.Add(cartItem);
        }

        m_cartItemCount_TMP.text = InventoryManager.Instance.m_cartClothingItems.Count <= 99 ? InventoryManager.Instance.m_cartClothingItems.Count.ToString() : "99+";
        m_cartItemsTotal_TMP.text = InventoryManager.Instance.GetCartTotalCost().ToString("C");
    }

    public void CloseCartWindow()
    {
        cartOpen = false;
        m_cartWindow.DOAnchorPosX(500, 0.5f);
        m_toggleCartArrow.DOLocalRotate(Vector3.zero, 0.5f);
    }

    public void ToggleInventoryWindow()
    {
        if (inventoryOpen)
            CloseInventoryWindow();
        else
            OpenInventoryWindow();
    }

    public void OpenInventoryWindow()
    {
        inventoryOpen = true;
        m_inventoryWindow.DOAnchorPosX(0, 0.5f);
        m_toggleInventoryArrow.DOLocalRotate(new Vector3(0, 0, -180), 0.5f);
        PushSidePanelToFront(0);

        UpdateInventoryWindow();
    }

    public void UpdateInventoryWindow()
    {
        if (inventoryOpen)
        {
            foreach (GameObject myItem in inventoryItemsDisplayed)
            {
                PoolManager.Instance.ResetObjInstance(myItem, PoolObjectType.InventoryItem);
            }
            inventoryItemsDisplayed.Clear();

            foreach (SO_ClothingItem item in InventoryManager.Instance.m_ownedClothingItems.Values)
            {
                GameObject invItem = PoolManager.Instance.GetPoolObject(PoolObjectType.InventoryItem);
                InventoryItemBehaviour invItemBehaviour = invItem.GetComponent<InventoryItemBehaviour>();
                invItemBehaviour.SetupInventoryItem(item.itemIcon, GetClothingTypeIcon(item.itemType));
                invItem.SetActive(true);
                inventoryItemsDisplayed.Add(invItem);
            }
        }
    }

    public void CloseInventoryWindow()
    {
        inventoryOpen = false;
        m_toggleInventoryArrow.DOLocalRotate(Vector3.zero, 0.5f);
        m_inventoryWindow.DOAnchorPosX(960, 0.5f);
    }

    public void ToggleBalanceWindow()
    {
        if (bankOpen)
            CloseBankBalance();
        else
            OpenBankBalanceWindow();
    }

    public void OpenBankBalanceWindow()
    {
        bankOpen = true;
        m_toggleBankArrow.DOLocalRotate(new Vector3(0,0,180), 0.5f);
        m_bankBalanceWindow.DOAnchorPosX(0, 0.5f);
        PushSidePanelToFront(1);
    }

    public void UpdateBankBalanceDisplay()
    {
        m_bankBalanceDisplay_TMP.text = InventoryManager.Instance.m_balance.ToString("C");
    }

    public void CloseBankBalance()
    {
        bankOpen = false;
        m_toggleBankArrow.DOLocalRotate(Vector3.zero, 0.5f);
        m_bankBalanceWindow.DOAnchorPosX(500, 0.5f);
    }

    private void CloseWindow(GameObject _window)
    {
        _window.SetActive(false);
    }

    public Sprite GetClothingTypeIcon(ClothingType _type)
    {
        foreach (ClothingTypeData data in m_clothingData)
            if (data.m_clothingType == _type) return data.m_clothingIconSprite;

        return m_missingSprite;
    }

    public void ShowPriceTag(float _price, string _itemName)
    {
        m_priceTagWindow.gameObject.SetActive(true);
        m_priceTag_TMP.text = _price.ToString("C");
        m_priceTagItemName_TMP.text = _itemName;
    }

    public void HidePriceTag()
    {
        m_priceTagWindow.gameObject.SetActive(false);
    }
}
