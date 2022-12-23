using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationArea : MonoBehaviour
{
    public Transform m_camOverrideTarget;

    private CameraBehaviour mainCam;

    private void OnEnable()
    {
        mainCam = Camera.main.GetComponent<CameraBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            mainCam.m_overrideTarget = m_camOverrideTarget;
            mainCam.m_override = true;
            UIManager.Instance.OpenCustomizationMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            mainCam.m_override = false;
            UIManager.Instance.CloseCustomizationWindow();
        }
    }
}
