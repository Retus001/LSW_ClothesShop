using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool inputEnabled = true;

    [HideInInspector]
    public Vector2 movementDirection;

    // Movement input event
    public delegate void Movement(Vector2 moveDir);
    public static event Movement OnMove;

    // Main menu key event
    public delegate void MainMenuPress();
    public static event MainMenuPress OnMainMenuPress;

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

            // Get Main menu input
            if (Input.GetButtonDown("Cancel"))
                if (OnMainMenuPress != null) OnMainMenuPress();
        }
    }
}
