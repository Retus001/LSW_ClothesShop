using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAssigner : MonoBehaviour
{
    public SO_ClothingItem clothingItem;

    public SpriteRendererBundle[] m_spriteRenderers_FWD;
    public SpriteRendererBundle[] m_spriteRenderers_SDE;

    public void AssignSpritesToRenderers()
    {
        if (clothingItem == null)
            return;

        foreach (SpriteRendererBundle rendBundle in m_spriteRenderers_FWD)
        {
            foreach (SpriteRenderer sprRend in rendBundle.renderers)
            {
                sprRend.sprite = clothingItem.itemSprites.GetSprite(rendBundle.section)[0];
            }
        }

        foreach (SpriteRendererBundle rendBundle in m_spriteRenderers_SDE)
        {
            foreach (SpriteRenderer sprRend in rendBundle.renderers)
            {
                sprRend.sprite = clothingItem.itemSprites.GetSprite(rendBundle.section)[1];
            }
        }
    }
}
