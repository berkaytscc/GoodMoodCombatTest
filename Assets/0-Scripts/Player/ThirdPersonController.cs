using Unity.Cinemachine;
using UnityEngine;

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

    private InputBridge _inputBridge;
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;
    private float _turnSmoothVelocity;
    private bool _isLockOnActive;

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
        ApplyGravity();
        Move();
        UpdateAnimator();

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            LockOnTarget(!_isLockOnActive);
        }
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

    private void Move()
    {
        Vector2 mi = _inputBridge != null
                     ? _inputBridge.MoveInput
                     : Vector2.zero;

        Vector3 inputDir = new Vector3(mi.x, 0f, mi.y).normalized;
        if (inputDir.sqrMagnitude < 0.01f)
            return;

        float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;

        float camYaw = _cinemachineCamera != null
                       ? _cinemachineCamera.transform.eulerAngles.y
                       : 0f;

        float targetAngle = inputAngle + camYaw;

        float smoothedAngle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref _turnSmoothVelocity,
            _turnSmoothTime
        );
        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
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
