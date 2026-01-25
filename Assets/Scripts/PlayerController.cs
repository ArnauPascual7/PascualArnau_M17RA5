using Unity.Android.Gradle.Manifest;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private Vector3 _currentBlendInput = Vector3.zero;

    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private Transform _cameraTarget;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _aimSpeed = 3f;
    [SerializeField] private float _jumpHeight = 8f;

    [Header("Ground Settings")]
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private LayerMask _groundLayers;

    [Header("Look Settings")]
    [SerializeField] private float _lookSense = 0.1f;
    [SerializeField] private float _lookLimitV = 70f;
    [SerializeField] private float _aimLookSenseMultiplier = 0.5f;
    [SerializeField] private float _aimCameraDistance = 2f;
    [SerializeField] private float _aimTransitionSpeed = 10f;

    private PlayerInputs _playerInputs;
    private CinemachineThirdPersonFollow _thirdPersonFollow;

    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;

    private float _normalCameraDistance;
    private float _verticalVelocity;
    private bool _isGrounded;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _thirdPersonFollow = _playerCamera.GetComponent<CinemachineThirdPersonFollow>();

        if (_thirdPersonFollow != null)
        {
            _normalCameraDistance = _thirdPersonFollow.CameraDistance;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GroundCheck();
        Gravity();
        Jump();
        Movement();
        Aim();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void GroundCheck()
    {
        float spherePositionY = transform.position.y - _characterController.height / 2 + _characterController.radius - _characterController.skinWidth;
        Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);

        _isGrounded = Physics.CheckSphere(spherePosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void Gravity()
    {
        _verticalVelocity -= _gravity * Time.deltaTime;

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
    }

    private void Jump()
    {
        if (_playerInputs.Jump && _isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt((_jumpHeight / 10f) * 3f * _gravity);
        }
    }

    private void Movement()
    {
        Vector3 cameraForward = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraForward * _playerInputs.Move.y + cameraRight * _playerInputs.Move.x;

        if (_playerInputs.Move != Vector2.zero)
        {
            if (_playerInputs.Sprint)
            {
                _currentBlendInput = Vector3.Lerp(_currentBlendInput, _playerInputs.Move * 1.5f, 4f * Time.deltaTime);
            }
            else
            {
                _currentBlendInput = Vector3.Lerp(_currentBlendInput, _playerInputs.Move * 1f, 4f * Time.deltaTime);
            }
        }
        else
        {
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, _playerInputs.Move * 0.5f, 4f * Time.deltaTime);
        }

        float currentSpeed = _moveSpeed;
        if (_playerInputs.Aim)
        {
            currentSpeed = _aimSpeed;
        }
        else if (_playerInputs.Sprint)
        {
            currentSpeed = _sprintSpeed;
        }

        animator.SetFloat("Speed", _currentBlendInput.magnitude);

        Vector3 velocity = movementDirection.normalized * currentSpeed;
        velocity.y = _verticalVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }

    private void Aim()
    {
        if (_thirdPersonFollow != null)
        {
            float cameraDistance = _playerInputs.Aim ? _aimCameraDistance : _normalCameraDistance;

            _thirdPersonFollow.CameraDistance = Mathf.Lerp(_thirdPersonFollow.CameraDistance, cameraDistance, _aimTransitionSpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        float lookSense = _lookSense * (_playerInputs.Aim ? _aimLookSenseMultiplier : 1f);

        _cameraRotation.x += _playerInputs.Look.x * lookSense;
        _cameraRotation.y -= _playerInputs.Look.y * lookSense;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -_lookLimitV, _lookLimitV);

        _playerTargetRotation.x += transform.eulerAngles.x + lookSense * _playerInputs.Look.x;

        _cameraTarget.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

        Quaternion targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
        float rotationSpeed = _playerInputs.Aim ? 15f : 10f;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        float spherePositionY = transform.position.y - _characterController.height / 2 + _characterController.radius - _characterController.skinWidth;
        Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spherePosition, _characterController.radius);
    }
}
