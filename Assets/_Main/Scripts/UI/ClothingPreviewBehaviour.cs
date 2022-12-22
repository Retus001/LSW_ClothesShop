using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingPreviewBehaviour : MonoBehaviour
{
    public Image m_clothingPreview;
    public TextMeshProUGUI m_clothingName_TMP;
    public GameObject m_hanger_Btn;
    public Button m_addToCart_Btn;
    public Button m_interact_Btn;
    private Animator anim;
    private bool showingTag = false;
    private SO_ClothingItem itemSO;

    private void OnEnable()
    {
        InventoryManager.OnUpdateCart += UpdatePreviewItem;
        InventoryManager.OnUpdateInventory += UpdatePreviewItem;
    }

    private void OnDisable()
    {
        InventoryManager.OnUpdateCart -= UpdatePreviewItem;
        InventoryManager.OnUpdateInventory -= UpdatePreviewItem;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        m_hanger_Btn.SetActive(false);
    }

    public void SetupClothingItem(SO_ClothingItem _item)
    {
        itemSO = _item;
        //itemSO.originalStorage = _storage;
        m_clothingPreview.sprite = _item.itemPreview;
        m_clothingName_TMP.text = _item.itemName;
        m_clothingName_TMP.rectTransform.sizeDelta = new Vector2(m_clothingName_TMP.preferredWidth, m_clothingName_TMP.rectTransform.sizeDelta.y);
        m_addToCart_Btn.onClick.RemoveAllListeners();
        m_addToCart_Btn.onClick.AddListener(() => InventoryManager.Instance.AddClothingItemToCart(_item));

        UpdatePreviewItem();
    }

    public void UpdatePreviewItem()
    {
        foreach (SO_ClothingItem item in InventoryManager.Instance.m_ownedClothingItems.Values)
        {
            if (itemSO == item)
            {
                m_clothingName_TMP.text = "Owned";
                m_addToCart_Btn.interactable = false;
                m_hanger_Btn.SetActive(false);
                m_interact_Btn.interactable = false;
                return;
            }
        }

        foreach (SO_ClothingItem item in InventoryManager.Instance.m_cartClothingItems.Values)
        {
            if (itemSO == item)
            {
                m_clothingName_TMP.text = "In Cart";
                m_addToCart_Btn.interactable = false;
                return;
            }
        }

        // Reset item from source
        m_clothingName_TMP.text = itemSO.itemName;
        m_addToCart_Btn.interactable = true;
        m_interact_Btn.interactable = true;
    }

    public void ClothingInteract()
    {
        anim.SetTrigger("RemoveClothing");
        m_hanger_Btn.SetActive(true);
    }

    public void HangerInteract()
    {
        anim.SetTrigger("ReturnClothing");
        m_hanger_Btn.SetActive(false);
    }

    public void PriceTagInteract()
    {
        if (!showingTag)
        {
            showingTag = true;
            UIManager.Instance.ShowPriceTag(itemSO.itemCost, itemSO.itemName);
        }
        else
        {
            showingTag = false;
            UIManager.Instance.HidePriceTag();
        }
    }
}
