using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBundle
{
    public Sprite forwardSpr;
    public Sprite backwardSpr;
    public Sprite sideSprite;

    public Sprite GetDirectionalSprite(Vector2 _dir)
    {
        if (_dir == Vector2.zero)
            return forwardSpr;

        if (_dir.y > 0)
            return backwardSpr;
        else if (_dir.y < 0)
            return forwardSpr;

        if (_dir.x > 0 || _dir.x < 0)
            return sideSprite;

        return forwardSpr;
    }

    public void SetDirectionalSprites(Sprite[] _sprites)
    {
        if(_sprites.Length != 3)
        {
            Debug.LogError("Not enough sprites to setup all directions");
            return;
        }
        
        if(_sprites[0] != null) forwardSpr = _sprites[0];
        if(_sprites[1] != null) sideSprite = _sprites[1];
        if(_sprites[2] != null) backwardSpr = _sprites[2];
    }
}

[Serializable]
public class SpriteRendererBundle
{
    public SpriteRenderer[] renderers;
    public ClothingSection section;
}

public class CharacterSpriteController : Singleton<CharacterSpriteController>
{
    public SpriteRendererBundle[] m_spriteRenderers;

    public ClothingSprites defaultSprites;

    public Animator anim;

    private Dictionary<ClothingSection, SpriteBundle> currentSpriteBundles = new Dictionary<ClothingSection, SpriteBundle>();

    private void OnEnable()
    {
        InventoryManager.OnUpdateEquipped += UpdateCurrentSprites;
        InputManager.OnChangedDirection += UpdateSpritesDirection;
        InputManager.OnMove += SetMovementBlendDirection;
    }

    private void Start()
    {
        for(int i = 0; i < defaultSprites.spritesData.Length; i++)
        {
            SpriteBundle newSpriteBundle = new SpriteBundle();

            newSpriteBundle.forwardSpr = defaultSprites.spritesData[i].sprite[0];
            newSpriteBundle.sideSprite = defaultSprites.spritesData[i].sprite[1];
            newSpriteBundle.backwardSpr = defaultSprites.spritesData[i].sprite[2];

            currentSpriteBundles.Add(defaultSprites.spritesData[i].section, newSpriteBundle);
        }

        Debug.Log("Finished setting up " + currentSpriteBundles.Count + " sprite bundles");
    }

    public void SetMovementBlendDirection(Vector2 _dir)
    {
        Vector2 clampedDirection = Vector2.zero;

        if (_dir.y > 0)
            clampedDirection = new Vector2(0, 1);
        else if (_dir.y < 0)
            clampedDirection = new Vector2(0, -1);
        else if (_dir.x > 0)
            clampedDirection = new Vector2(1, 0);
        else if (_dir.x < 0)
            clampedDirection = new Vector2(-1, 0);

        anim.SetFloat("MovSpeedX", clampedDirection.x);
        anim.SetFloat("MovSpeedY", clampedDirection.y);
    }

    public void UpdateSpritesDirection(Vector2 _dir)
    {
        // Set all sprites from direction
        foreach(SpriteRendererBundle rendBundle in m_spriteRenderers)
        {
            foreach(SpriteRenderer sprRend in rendBundle.renderers)
            {
                sprRend.sprite = currentSpriteBundles[rendBundle.section].GetDirectionalSprite(_dir);
            }
        }
    }

    public void UpdateCurrentSprites()
    {
        ClothingSprites selectedSprites = null;

        // Check if the one piece is real
        SO_ClothingItem onePiece = null;
        bool foundOnePiece = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.FULL, out onePiece);
        if (foundOnePiece) selectedSprites = onePiece.itemSprites;

        if (!foundOnePiece)
        {
            // Check for equipped top
            SO_ClothingItem topClothing = null;
            selectedSprites = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.TOP, out topClothing) ? topClothing.itemSprites : defaultSprites;
        }

        // Asign top sprites [ Torso / Arms / Forearms / Hands ]
        currentSpriteBundles[ClothingSection.TORSO].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.TORSO));
        currentSpriteBundles[ClothingSection.ARMS].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.ARMS));
        currentSpriteBundles[ClothingSection.FOREARMS].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.FOREARMS));

        if (!foundOnePiece)
        {
            // Check for equipped bottom
            SO_ClothingItem bottomClothing = null;
            selectedSprites = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.BOTTOM, out bottomClothing) ? bottomClothing.itemSprites : defaultSprites;
        }

        // Asign bottom sprites [ Hips / Legs / Calves ]
        currentSpriteBundles[ClothingSection.HIPS].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.HIPS));
        currentSpriteBundles[ClothingSection.LEGS].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.LEGS));
        currentSpriteBundles[ClothingSection.CALVES].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.CALVES));

        // Check for equipped footwear
        SO_ClothingItem footwearClothing = null;
        selectedSprites = InventoryManager.Instance.m_equippedItems.TryGetValue(ClothingType.FOOTWEAR, out footwearClothing) ? footwearClothing.itemSprites : defaultSprites;

        // Asign footwear sprites [ Feet ]
        currentSpriteBundles[ClothingSection.FEET].SetDirectionalSprites(selectedSprites.GetSprite(ClothingSection.FEET));

        // Check for equipped overtop

        // Asign overtop sprites [ OverTorso / Arms / Forearms ]

        // Asign overtop mask to torso sprite

        UpdateSpritesDirection(Vector2.down);
    }

    public SpriteRendererBundle GetSpriteRenderers(ClothingSection _section)
    {
        foreach(SpriteRendererBundle rendBundle in m_spriteRenderers)
        {
            if (rendBundle.section == _section) return rendBundle;
        }

        return null;
    }

    public void SetTemporaryItem(SO_ClothingItem _item)
    {
        bool setTorso = false, setHips = false, setLegs = false, setCalves = false, setArms = false, setForearms = false, setFeet = false;

        switch (_item.itemType)
        {
            case ClothingType.TOP: setTorso = true; setArms = true; setForearms = true; break;
            case ClothingType.BOTTOM: setHips = true; setLegs = true; setCalves = true; break;
            case ClothingType.FULL: setTorso = true; setArms = true; setForearms = true; setHips = true; setLegs = true; setCalves = true; break;
            case ClothingType.FOOTWEAR: setFeet = true; break;
        }

        if (setTorso)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.TORSO).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.TORSO)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.TORSO)[0];
        if (setHips)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.HIPS).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.HIPS)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.HIPS)[0];
        if (setArms)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.ARMS).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.ARMS)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.ARMS)[0];
        if (setForearms)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.FOREARMS).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.FOREARMS)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.FOREARMS)[0];
        if (setLegs)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.LEGS).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.LEGS)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.LEGS)[0];
        if (setCalves)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.CALVES).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.CALVES)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.CALVES)[0];
        if (setFeet)
            foreach (SpriteRenderer rend in GetSpriteRenderers(ClothingSection.FEET).renderers)
                if (_item.itemSprites.GetSprite(ClothingSection.FEET)[0] != null) rend.sprite = _item.itemSprites.GetSprite(ClothingSection.FEET)[0];
    }

    private void OnDisable()
    {
        InventoryManager.OnUpdateEquipped -= UpdateCurrentSprites;
        InputManager.OnChangedDirection -= UpdateSpritesDirection;
        InputManager.OnMove -= SetMovementBlendDirection;
    }
}
