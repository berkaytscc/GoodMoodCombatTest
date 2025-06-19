using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _gravity = -9.81f;

    private InputBridge _inputBridge;
    private CharacterController _controller;
    private Vector3 _velocity;
    private float _turnSmoothVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        ApplyGravity();
        MoveAndRotate();
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void MoveAndRotate()
    {
        Vector3 inputDir = new Vector3(_inputBridge.MoveInput.x, 0f, _inputBridge.MoveInput.y).normalized;
        if (inputDir.magnitude < 0.1f)
            return;

        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                            + _cameraTransform.eulerAngles.y;
        float smoothedAngle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref _turnSmoothVelocity,
            _turnSmoothTime
        );

        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _controller.Move(moveDir.normalized * _walkSpeed * Time.deltaTime);
    }
}
