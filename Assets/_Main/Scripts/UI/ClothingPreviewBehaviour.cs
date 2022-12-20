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
    private float price;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        m_hanger_Btn.SetActive(false);
    }

    public void SetupClothingItem(Sprite _clothingPreview, float _cost, string _clothingName)
    {
        m_clothingPreview.sprite = _clothingPreview;
        price = _cost;
        m_clothingName_TMP.text = _clothingName;
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
        if (!UIManager.Instance.m_priceTagWindow.activeSelf)
            UIManager.Instance.ShowPriceTag("$ " + price.ToString());
        else
            UIManager.Instance.HidePriceTag();
    }
}
