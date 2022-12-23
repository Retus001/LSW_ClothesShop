using System;
using UnityEngine;

[CreateAssetMenu(fileName = "newClothingItem", menuName = "ClothingItem", order = 0)]

public class SO_ClothingItem : ScriptableObject
{
    public string itemName;
    public ClothingType itemType;
    public ClothingSprites itemSprites;
    public Sprite itemIcon;
    public Sprite itemPreview;
    public float itemCost;
}

public enum ClothingType
{
    OVERTOP,
    TOP,
    BOTTOM,
    FULL,
    FOOTWEAR
}

public enum ClothingSection
{
    HEAD,
    TORSO,
    HIPS,
    ARMS,
    FOREARMS,
    HANDS,
    LEGS,
    CALVES,
    FEET
}

[Serializable]
public class ClothingSprites
{
    [Serializable]
    public struct ClothingSpriteData {
        public Sprite sprite;
        public ClothingSection section;
    }

    public ClothingSpriteData[] spritesData;

    public Sprite GetSprite(ClothingSection _section)
    {
        foreach(ClothingSpriteData spr in spritesData)
        {
            if (spr.section == _section)
                return spr.sprite;
        }
        return null;
    }
}
