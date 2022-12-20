using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingStorage : MonoBehaviour
{
    // Data
    public ClothingType m_clothingType;
    public List<SO_ClothingItem> m_clothingItems = new List<SO_ClothingItem>();

    // References
    public Transform m_camTarget;
    private CameraBehaviour mainCam;
    private GameObject popUpObj;

    private bool previewingStorage = false;
    private bool storageOpen = false;

    private void OnEnable()
    {
        mainCam = Camera.main.GetComponent<CameraBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            if(!previewingStorage)
                ShowStorageData();
        }
    }

    private void OnTriggerExit2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            if (previewingStorage)
                HideStorageData();
        }
    }

    private void ShowStorageData()
    {
        previewingStorage = true;

        // Instantiate pooled popup
        popUpObj = PoolManager.Instance.GetPoolObject(PoolObjectType.IGPopupSmall);
        PopUpBehaviour popUp = popUpObj.GetComponent<PopUpBehaviour>();

        popUp.buildPopUp(m_clothingType, this);
        popUpObj.transform.position = transform.position;
        popUpObj.SetActive(true);
    }

    private void HideStorageData()
    {
        previewingStorage = false;

        // Reset pooled popup
        PoolManager.Instance.ResetObjInstance(popUpObj, PoolObjectType.IGPopupSmall);
    }

    public void OpenStorage()
    {
        storageOpen = true;

        HideStorageData();

        // Override main camera and set size
        mainCam.m_overrideTarget = m_camTarget;
        mainCam.m_override = true;
        mainCam.SetCamSize(3);

        // Open storage window and setup UI
        UIManager.Instance.OpenStorageWindow(this);
    }

    public void CloseStorage()
    {
        storageOpen = false;

        ShowStorageData();

        mainCam.m_override = false;
        mainCam.ResetCamSize();
    }
}
