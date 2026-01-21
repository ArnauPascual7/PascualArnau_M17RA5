using Unity.Android.Gradle.Manifest;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _jumpHeight = 8f;

    [Header("Ground Settings")]
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private LayerMask _groundLayers;
    
    private PlayerInputs _playerInputs;

    private float _verticalVelocity;
    private bool _isGrounded;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        GroundCheck();
        Gravity();
        Jump();
        Movement();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);
        float sphereRadius = _characterController.radius + _characterController.skinWidth;

        _isGrounded = Physics.CheckSphere(spherePosition, sphereRadius, _groundLayers, QueryTriggerInteraction.Ignore);
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

        float currentSpeed = _playerInputs.Sprint ? _sprintSpeed : _moveSpeed;

        Vector3 velocity = movementDirection.normalized * currentSpeed;
        velocity.y = _verticalVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z), _characterController.radius + _characterController.skinWidth);
    }
}
