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
        // References
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        DOTween.Init();
        Tweener followTargetTween = transform.DOMove(new Vector3(m_target.position.x, m_target.position.y, transform.position.z), 1f / m_followSpeed);
        followTargetTween.OnUpdate(() =>
        {
            if (m_override)
            {
                if (Vector3.Distance(transform.position, m_overrideTarget.position) > 0.2f)
                {
                    followTargetTween.ChangeEndValue(new Vector3(m_overrideTarget.position.x, m_overrideTarget.position.y, transform.position.z), true);
                }
            } else
            {
                if (Vector3.Distance(transform.position, m_target.position) > 0.2f)
                {
                    followTargetTween.ChangeEndValue(new Vector3(m_target.position.x, m_target.position.y, transform.position.z), true);
                }
            }
        });
    }

    public void SetCamSize(float _size)
    {
        cam.DOOrthoSize(_size, 1f);
    }

    public void ResetCamSize()
    {
        cam.DOOrthoSize(10, 1f);
    }
}
