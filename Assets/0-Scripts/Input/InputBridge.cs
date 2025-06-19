using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBridge : MonoBehaviour
{
    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _canMove;
    private bool _canLook;

    private void Awake()
    {
        EnableMovement(true);
        EnableLooking(true);
    }

    private void Start()
    {
        SetCursorState(false);
    }

    public void EnableMovement(bool enable) => _canMove = enable;
    public void EnableLooking(bool enable) => _canLook = enable;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_canMove) return;
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!_canLook) return;
        _lookInput = context.ReadValue<Vector2>();
    }

    private void SetCursorState(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Confined;
    }
}
