using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode m_MoveForward_Key;
    public KeyCode m_MoveBackward_Key;
    public KeyCode m_MoveRight_Key;
    public KeyCode m_MoveLeft_Key;

    [HideInInspector]
    public bool moveF, moveB, moveR, moveL;

    void Update()
    {
        moveF = Input.GetKey(m_MoveForward_Key);
        moveB = Input.GetKey(m_MoveBackward_Key);
        moveR = Input.GetKey(m_MoveRight_Key);
        moveL = Input.GetKey(m_MoveLeft_Key);
    }
}
