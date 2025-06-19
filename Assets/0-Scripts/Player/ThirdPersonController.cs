using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(InputBridge), typeof(Animator))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Main Camera that has a CinemachineBrain on it")]
    [SerializeField] private Camera _cinemachineCamera;
    [SerializeField] private CinemachineCamera _vCamFreeLook;
    [SerializeField] private CinemachineCamera _vCamLockOn;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Animation Settings")]
    [Tooltip("Damp time for blending animation parameters")]
    [SerializeField] private float _animDampTime = 0.1f;

    [SerializeField] private Transform lockOnTarget;

    // --- Combo/Attack System ---
    [Header("Combat Settings")]
    [SerializeField] private int _maxCombo = 3;
    [SerializeField] private float _comboResetTime = 1.0f; // Time window to chain next attack

    private InputBridge _inputBridge;
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;
    private float _turnSmoothVelocity;
    private bool _isLockOnActive;
    private int _currentCombo = 0;
    private float _lastAttackTime = -999f;
    private bool _isAttacking = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputBridge = GetComponent<InputBridge>();
        _animator = GetComponent<Animator>();

        if (_cinemachineCamera == null && Camera.main != null)
            _cinemachineCamera = Camera.main;
    }

    private void Update()
    {
        //TODO: change to new input system later
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            _isLockOnActive = !_isLockOnActive;

            if (_vCamFreeLook != null && _vCamLockOn != null)
            {
                _vCamLockOn.Priority = _vCamFreeLook.Priority + (_isLockOnActive ? 1 : -1);
            }
        }

        ApplyGravity();
        UpdateAnimator();

        if (_isLockOnActive)
            StrafeMove();
        else
            FreeMove();
    }

    public void LockOnTarget(bool active)
    {
        _vCamLockOn.Priority = _vCamFreeLook.Priority + (active ? 1 : - 1);
        _isLockOnActive = active;
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void FreeMove()
    {
        Vector2 mi = _inputBridge.MoveInput;
        Vector3 inputDir = new Vector3(mi.x, 0f, mi.y).normalized;
        if (inputDir.sqrMagnitude < 0.01f) return;

        float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
        float camYaw = _cinemachineCamera.transform.eulerAngles.y;
        float targetAngle = inputAngle + camYaw;

        float smooth = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref _turnSmoothVelocity,
            _turnSmoothTime
        );
        transform.rotation = Quaternion.Euler(0f, smooth, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _controller.Move(moveDir * _walkSpeed * Time.deltaTime);
    }

    private void StrafeMove()
    {
        if (lockOnTarget == null) return;

        Vector2 mi = _inputBridge.MoveInput;
        Vector3 toTarget = lockOnTarget.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);

        Vector3 forward = toTarget.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 moveDir = forward * mi.y + right * mi.x;

        if (moveDir.sqrMagnitude > 0.01f)
            _controller.Move(moveDir * _walkSpeed * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        Vector2 mi = _inputBridge.MoveInput;
        Vector3 camF = _cinemachineCamera.transform.forward;
        camF.y = 0; camF.Normalize();
        Vector3 camR = _cinemachineCamera.transform.right;
        camR.y = 0; camR.Normalize();

        Vector3 worldMove = camR * mi.x + camF * mi.y;
        float speed = Mathf.Clamp01(worldMove.magnitude);

        float h = 0f, v = 0f;
        if (speed > 0.01f)
        {
            Vector3 localDir = transform.InverseTransformDirection(worldMove.normalized);
            h = localDir.x;
            v = localDir.z;
        }

        _animator.SetFloat("Horizontal", h, _animDampTime, Time.deltaTime);
        _animator.SetFloat("Vertical", v, _animDampTime, Time.deltaTime);
        _animator.SetFloat("Speed", speed);
    }
}
