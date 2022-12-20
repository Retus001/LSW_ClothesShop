using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBehaviour : MonoBehaviour
{
    // References
    public Image m_typeIcon;

    private ClothingStorage storage;

    public void buildPopUp(ClothingType _clothingType, ClothingStorage _storage)
    {
        m_typeIcon.sprite = UIManager.Instance.GetClothingTypeIcon(_clothingType);
        storage = _storage;
    }

    public void Interact()
    {
        storage.OpenStorage();
    }
}
