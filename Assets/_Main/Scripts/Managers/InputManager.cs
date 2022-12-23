using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool inputEnabled = true;

    [HideInInspector]
    public Vector2 movementDirection;

    private Vector2 prevDir = Vector2.zero;

    // Movement input event
    public delegate void Movement(Vector2 moveDir);
    public static event Movement OnMove;

    // Main menu key event
    public delegate void MainMenuPress();
    public static event MainMenuPress OnMainMenuPress;

    // Changed direction event
    public delegate void ChangedDirection(Vector2 _newDir);
    public static event ChangedDirection OnChangedDirection;

    // Interaction input event
    //public delegate void Interact();
    //public static event Interact OnInteract;

    void Update()
    {
        if (inputEnabled)
        {
            // Get movement input
            movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            if(prevDir != movementDirection)
            {
                if (OnChangedDirection != null) OnChangedDirection(movementDirection);
                prevDir = movementDirection;
            }

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
