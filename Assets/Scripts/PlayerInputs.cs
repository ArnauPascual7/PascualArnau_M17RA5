using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public InputSystem_Actions InputActions { get; private set; }

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    public bool Sprint { get; private set; }
    public bool Jump { get; private set; }
    public bool Aim { get; private set; }
    public bool Fire { get; private set; }
    public bool Interact { get; private set; }
    public bool Dance { get; private set; }

    private void OnEnable()
    {
        EnableInputs();
    }

    private void EnableInputs()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();

        InputActions.Player.Enable();
        InputActions.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    private void DisableInputs()
    {
        InputActions.Player.Disable();
        InputActions.Player.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        Jump = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Fire = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump = true;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        Sprint = context.ReadValueAsButton();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Aim = context.ReadValueAsButton();
    }

    public void OnDance(InputAction.CallbackContext context)
    {
        Dance = !Dance;
    }
}
