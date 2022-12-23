using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationBehaviour : MonoBehaviour
{
    public SpriteRenderer m_head_spr;
    public SpriteRenderer m_torso_spr;
    public SpriteRenderer m_hips_spr;
    public SpriteRenderer[] m_arms_spr;
    public SpriteRenderer[] m_forearms_spr;
    public SpriteRenderer[] m_hands_spr;
    public SpriteRenderer[] m_legs_spr;
    public SpriteRenderer[] m_calves_spr;
    public SpriteRenderer[] m_feet_spr;

    public ClothingSprites defaultSprites;

    private void OnEnable()
    {
        InventoryManager.OnUpdateEquipped += UpdateCustomizationElements;
    }

    public void UpdateCustomizationElements()
    {
        ClothingSprites selectedSprites = null;

        // Check if the one piece is real
        SO_ClothingItem onePiece = null;
        bool foundOnePiece = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.FULL, out onePiece);
        if(foundOnePiece) selectedSprites = onePiece.itemSprites;

        // Select sprites for top
        // If one piece wasn't found select sprites from equipped top instead
        if (!foundOnePiece)
        {
            // Check for equipped top
            SO_ClothingItem topClothes = null;
            bool foundTop = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.TOP, out topClothes);
            selectedSprites = foundTop ? topClothes.itemSprites : defaultSprites;
        }

        // Asign top sprites [ Torso / Arms / Forearms / Hands ]
        m_torso_spr.sprite = selectedSprites.GetSprite(ClothingSection.TORSO) != null ? selectedSprites.GetSprite(ClothingSection.TORSO) : defaultSprites.GetSprite(ClothingSection.TORSO);
        foreach (SpriteRenderer sprR in m_arms_spr)
            sprR.sprite = selectedSprites.GetSprite(ClothingSection.ARMS) != null ? selectedSprites.GetSprite(ClothingSection.ARMS) : defaultSprites.GetSprite(ClothingSection.ARMS);
        foreach (SpriteRenderer sprR in m_forearms_spr)
            sprR.sprite = selectedSprites.GetSprite(ClothingSection.FOREARMS) != null ? selectedSprites.GetSprite(ClothingSection.FOREARMS) : defaultSprites.GetSprite(ClothingSection.FOREARMS);
        foreach (SpriteRenderer sprR in m_hands_spr)
            sprR.sprite = selectedSprites.GetSprite(ClothingSection.HANDS) != null ? selectedSprites.GetSprite(ClothingSection.HANDS) : defaultSprites.GetSprite(ClothingSection.HANDS);

        // Select sprites for bottom
        // If one piece wasn't found select sprites from equipped bottom instead
        if (!foundOnePiece)
        {
            // Check for equipped bottom
            SO_ClothingItem bottomClothes = null;
            bool foundBot = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.BOTTOM, out bottomClothes);
            selectedSprites = foundBot ? bottomClothes.itemSprites : defaultSprites;
        }

        // Asign bottom sprites [ Hips / Legs / Calves ]
        m_hips_spr.sprite = selectedSprites.GetSprite(ClothingSection.HIPS) != null ? selectedSprites.GetSprite(ClothingSection.HIPS) : defaultSprites.GetSprite(ClothingSection.HIPS);
        foreach (SpriteRenderer sprR in m_legs_spr)
            sprR.sprite = selectedSprites.GetSprite(ClothingSection.LEGS) != null ? selectedSprites.GetSprite(ClothingSection.LEGS) : defaultSprites.GetSprite(ClothingSection.LEGS);
        foreach (SpriteRenderer sprR in m_calves_spr)
            sprR.sprite = selectedSprites.GetSprite(ClothingSection.CALVES) != null ? selectedSprites.GetSprite(ClothingSection.CALVES) : defaultSprites.GetSprite(ClothingSection.CALVES);

        // Check for equipped footwear
        SO_ClothingItem shoes = null;
        bool foundFootwear = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.FOOTWEAR, out shoes);
        selectedSprites = foundFootwear ? shoes.itemSprites : defaultSprites;

        // Asign footwear sprites [ Feet ]
        foreach (SpriteRenderer sprR in m_feet_spr)
            sprR.sprite = selectedSprites.GetSprite(ClothingSection.FEET) != null ? selectedSprites.GetSprite(ClothingSection.FEET) : defaultSprites.GetSprite(ClothingSection.FEET);
    
        // TODO: Check for equipped overtop [ These sprites will override arms, and forearms if available and render on top of torso, torso needs to have a mask assigned
    }

    private void OnDisable()
    {
        InventoryManager.OnUpdateEquipped -= UpdateCustomizationElements;
    }
}
