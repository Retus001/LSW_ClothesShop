using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool inputEnabled = true;

    [SerializeField]
    private Vector2 movementDirection;

    // Movement input event
    public delegate void Movement(Vector2 moveDir);
    public static event Movement OnMove;

    // Interaction input event
    //public delegate void Interact();
    //public static event Interact OnInteract;

    void Update()
    {
        if (inputEnabled)
        {
            // Get movement input
            movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            // Trigger movement input event
            if (OnMove != null) OnMove(movementDirection);

            // Get Interaction Input
            //if(Input.GetButtonDown("Interact"))
            //    if (OnInteract != null) OnInteract();
        }
    }
}
