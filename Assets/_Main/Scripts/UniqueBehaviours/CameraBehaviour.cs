using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{
    [Min(0.1f)]
    public float m_followSpeed = 1;
    public Transform m_target;
    public Transform m_overrideTarget;
    public bool m_override;

    // References
    private Camera cam;

    private void OnEnable()
    {
        // Event Subscriptions
        PlayerController.OnPlayerMoving += UpdateCamPosition;

        // References
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        DOTween.Init();
    }

    public void UpdateCamPosition()
    {
        transform.DOMove(new Vector3(
            m_override? m_overrideTarget.position.x : m_target.position.x,
            m_override ? m_overrideTarget.position.y : m_target.position.y, 
            transform.position.z), 
            1f / m_followSpeed);
    }

    public void SetCamSize(float _size)
    {
        cam.DOOrthoSize(_size, 1f);
    }

    public void ResetCamSize()
    {
        cam.DOOrthoSize(5, 1f);
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMoving -= UpdateCamPosition;
    }
}
