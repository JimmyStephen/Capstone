using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCurser : MonoBehaviour
{
    //https://www.youtube.com/watch?v=Y3WNwl1ObC8
    [SerializeField] PlayerInput playerInput;
    [SerializeField] RectTransform cursorTransform;
    [SerializeField] float speed = 1000f;
    [SerializeField] RectTransform canvasTransform;

    private Mouse virtualMouse;
    private bool prevMouseState = false;

    private void OnEnable()
    {
        if(virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);
        
        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }
        
        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        Debug.Log("Disable Cursor");
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        //movement
        if (virtualMouse == null || Gamepad.current == null) return;

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= speed * Time.deltaTime;
        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        //on click
        bool aButtonIsPressed = Gamepad.current.aButton.isPressed;
        if (prevMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            prevMouseState = aButtonIsPressed;
        }
        AnchorPosition(newPosition);
    }

    private void AnchorPosition(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, position, null, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }
}
