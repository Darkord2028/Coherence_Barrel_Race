using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    #region Input Flags

    public Vector2 MovementInput { get; private set; }
    public bool isReadyInput { get; private set; }
    public bool AccelerateInput { get; private set; }

    #endregion

    #region Private Variables

    private Vector2 lastTouchPosition;
    private Vector2 currentTouchPosition;

    #endregion

    #region SeializedField Variables

    [SerializeField] float swipeThreshold = 5f;

    #endregion

    #region Input Actions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnReadyInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isReadyInput = true;
        }
    }

    public void OnAccelerateInput(InputAction.CallbackContext context)
    {
        if (context.performed) { AccelerateInput = true; }
        else { AccelerateInput = false; }
    }

    public void OnTouchPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AccelerateInput = true;
            lastTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (context.canceled)
        {
            AccelerateInput = false;
            MovementInput = Vector2.zero;
        }
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        if (!AccelerateInput) return; // Only care when finger is held

        currentTouchPosition = context.ReadValue<Vector2>();
        Vector2 delta = currentTouchPosition - lastTouchPosition;

        // Normalize for directional input if needed
        if (delta.magnitude > swipeThreshold) // Minimum threshold to avoid noise
        {
            MovementInput = delta.normalized;
            lastTouchPosition = currentTouchPosition;
        }
        else
        {
            MovementInput = Vector2.zero;
        }
    }

    #endregion

    #region Use Input Methods

    public void UseReadyInput() => isReadyInput = false;

    #endregion

}
