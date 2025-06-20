using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBridge : MonoBehaviour
{
    public event Action OnAttackPerformed;
    public event Action OnStrafePerformed;

    public Vector2 MoveInput => _moveInput;

    private Vector2 _moveInput;
    private bool _canMove;
    private bool _canAttack;

    private void Awake()
    {
        EnableMovement(true);
        EnableAttacking(true);
    }

    private void Start()
    {
        SetCursorState(false);
    }

    public void EnableMovement(bool enable)
    {
        if(!enable) _moveInput = Vector2.zero;
        _canMove = enable;
    }
    public void EnableAttacking(bool enable) => _canAttack = enable;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_canMove) return;
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!_canAttack) return;

        if (ctx.started)
            OnAttackPerformed?.Invoke();
    }

    public void OnStrafe(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            OnStrafePerformed?.Invoke();
    }

    private void SetCursorState(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Confined;
    }
}
