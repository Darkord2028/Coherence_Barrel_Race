using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    #region Input Flags

    public Vector2 MovementInput { get; private set; }
    public bool isReadyInput { get; private set; }

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

    #endregion

    #region Use Input Methods

    public void UseReadyInput() => isReadyInput = false;

    #endregion

}
