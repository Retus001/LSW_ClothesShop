using UnityEngine;

[CreateAssetMenu(fileName = "newClothingItem", menuName = "ClothingItem", order = 0)]

public class SO_ClothingItem : ScriptableObject
{
    public string itemName;
    public ClothingType itemType;
    public Sprite itemSprite;
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
