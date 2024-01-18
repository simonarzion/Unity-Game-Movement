using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions {
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;
    public bool IsSprinting;
    public Action OnJumpPerformed;
    private Controls controls;

    private void OnEnable() {
        if (controls != null) {
            return;
        }

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    public void OnLook(InputAction.CallbackContext context) {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (!context.performed) {
            return;
        }

        OnJumpPerformed?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context) {
        IsSprinting = MoveComposite.sqrMagnitude > 0f && context.ReadValueAsButton();
    }
}
