using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CursorMovement : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    //[SerializeField] RectTransform cursorTransform;
    [SerializeField] float speed = 1000f;
    //[SerializeField] RectTransform canvasTransform;
    [SerializeField] float padding = 35f;

    Vector2 currentPosition;

    private bool prevMouseState = false;

    private void OnEnable()
    {
        Cursor.visible = true;
        currentPosition = Vector2.zero;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Gamepad.current.leftShoulder.isPressed || Mouse.current.rightButton.isPressed)
        {
            return;
        }
        //Get the position of the gamepad
        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        //multiply it by how fast it should be going
        deltaValue *= speed * Time.deltaTime * (Gamepad.current.leftTrigger.IsPressed() ? 2 : 1);

        //get the new position
        Vector2 newPosition = currentPosition + deltaValue;

        //clamp the area the mouse can go
        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        //Set the mouse to the position of the gamepad

        Mouse.current.WarpCursorPosition(newPosition);
        currentPosition = newPosition;

        //on click
        bool aButtonIsPressed = Gamepad.current.aButton.isPressed;
        if (prevMouseState != aButtonIsPressed)
        {
            Mouse.current.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(Mouse.current, mouseState);
            prevMouseState = aButtonIsPressed;
        }
    }
}
