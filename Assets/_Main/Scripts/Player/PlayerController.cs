using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float m_speed;
    public Vector2 m_camTargetOffset;

    [Header("References")]
    public Transform m_camTarget;
    private Rigidbody2D rig;

    private Vector2 newVelocity;
    private float oldTime;

    // Player moving event
    public delegate void PlayerMoving();
    public static event PlayerMoving OnPlayerMoving;

    private void OnEnable()
    {
        // Event Subscriptions
        InputManager.OnMove += MovePlayer;

        // References
        rig = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector2 _dir)
    {
        // Set velocity to player rig
        newVelocity = _dir * m_speed;
        rig.velocity = newVelocity;

        // Set camera target position based on movement direction
        if(_dir != Vector2.zero)
        {
            m_camTarget.position = transform.position + new Vector3(0, 1.5f, 0) + new Vector3(_dir.x * m_camTargetOffset.x, _dir.y * m_camTargetOffset.y, 0);
        }

        // Handle footsteps audio
        if (_dir != Vector2.zero)
        {
            if (Time.time >= oldTime + (m_speed / 8))
            {
                AudioManager.Instance.SpawnAudioSource(transform.position, AudioType.footsteps);
                oldTime = Time.time;
            }
        }

        // Trigger player moving event
        if (OnPlayerMoving != null) OnPlayerMoving();
    }

    private void OnDisable()
    {
        InputManager.OnMove -= MovePlayer;
    }
}
