using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBridge : MonoBehaviour
{
    public Vector2 MoveInput => _moveInput;

    private Vector2 _moveInput;
    private bool _canMove;

    private void Awake()
    {
        EnableMovement(true);
    }

    private void Start()
    {
        SetCursorState(false);
    }

    public void EnableMovement(bool enable) => _canMove = enable;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_canMove) return;
        _moveInput = context.ReadValue<Vector2>();
    }

    private void SetCursorState(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Confined;
    }
}
