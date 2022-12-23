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
    //public delegate void PlayerMoving();
    //public static event PlayerMoving OnPlayerMoving;

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
            m_camTarget.position = transform.position + new Vector3(_dir.x * m_camTargetOffset.x, m_camTargetOffset.y, 0);
        }

        // Handle footsteps audio
        if (_dir != Vector2.zero)
        {
            if (Time.time >= oldTime + (4f / m_speed))
            {
                AudioManager.Instance.SpawnAudioSource(transform.position, AudioType.footsteps);
                oldTime = Time.time;
            }
        }
    }

    private void OnDisable()
    {
        InputManager.OnMove -= MovePlayer;
    }
}
