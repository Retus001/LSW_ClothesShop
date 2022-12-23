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
    private float defaultCamSize;
    private Vector3 targetPos;

    private void OnEnable()
    {
        // References
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        DOTween.Init();
        Tweener followTargetTween = transform.DOMove(new Vector3(m_target.position.x, m_target.position.y, transform.position.z), 1f / m_followSpeed);
        followTargetTween.OnUpdate(() =>
        {
            targetPos = m_override ? m_overrideTarget.position : m_target.position;

            if (Vector3.Distance(transform.position, targetPos) > 0.2f)
            {
                followTargetTween.ChangeEndValue(new Vector3(targetPos.x, targetPos.y, transform.position.z), true);
            }
        });

        defaultCamSize = cam.orthographicSize;
    }

    public void SetCamSize(float _size)
    {
        cam.DOOrthoSize(_size, 1f);
    }

    public void ResetCamSize()
    {
        cam.DOOrthoSize(defaultCamSize, 1f);
    }
}
