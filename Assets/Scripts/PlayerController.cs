using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(CharacterController), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CinemachineCamera _danceCamera;
    [SerializeField] private GameObject _partyHat;

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
    private PlayerState _playerState;
    private CinemachineThirdPersonFollow _thirdPersonFollow;

    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;

    private float _timeSinceLastGrounded = 0f;
    private bool _jumpedLastFrame = false;
    private float _normalCameraDistance;
    private float _verticalVelocity;
    private bool _isGrounded;
    private float _stepOffset;
    private Vector3 _groundShperePos;

    public bool objCollected = false;

    private PlayerMoveState _lastMovementState = PlayerMoveState.Falling;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _playerState = GetComponent<PlayerState>();
        _thirdPersonFollow = _playerCamera.GetComponent<CinemachineThirdPersonFollow>();

        if (_thirdPersonFollow != null)
        {
            _normalCameraDistance = _thirdPersonFollow.CameraDistance;
        }

        _stepOffset = _characterController.stepOffset;
    }

    private void Start()
    {
        GameManager.Instance.SetCursorState(false);
    }

    private void Update()
    {
        UpdateMoveState();

        GroundCheck();
        Gravity();
        
        if (_playerState.CurrentPlayerMoveState == PlayerMoveState.Dancing)
        {
            _danceCamera.Priority = 1;
        }
        else
        {
            _danceCamera.Priority = -1;

            Jump();
            Movement();
            Aim();
        }
    }

    private void LateUpdate()
    {
        Rotation();
    }

    /*private void UpdateMoveState()
    {
        _lastMovementState = _playerState.CurrentPlayerMoveState;

        bool isMoving = _playerInputs.Move != Vector2.zero;
        bool isMovingLaterlally = IsMovingLaterally();
        bool isSprinting = _playerInputs.Sprint && isMovingLaterlally;

        PlayerMoveState state;
        if (isSprinting)
        {
            state = PlayerMoveState.Sprinting;
        }
        else if (isMovingLaterlally || isMoving)
        {
            state = PlayerMoveState.Walking;
        }
        else
        {
            state = PlayerMoveState.Idle;
        }

        _playerState.SetPlayerMoveState(state);

        if ((!_isGrounded || _jumpedLastFrame) && _characterController.velocity.y > 0)
        {
            _playerState.SetPlayerMoveState(PlayerMoveState.Jumping);
            _jumpedLastFrame = false;
            _characterController.stepOffset = 0f;
        }
        else
        {
            _characterController.stepOffset = _stepOffset;
        }
    }*/
    private void UpdateMoveState()
    {
        _lastMovementState = _playerState.CurrentPlayerMoveState;

        bool isMoving = _playerInputs.Move != Vector2.zero;
        bool isMovingLaterlally = IsMovingLaterally();
        bool isSprinting = _playerInputs.Sprint && isMovingLaterlally;

        if (_jumpedLastFrame)
        {
            _characterController.stepOffset = 0f;
            _playerState.SetPlayerMoveState(PlayerMoveState.Jumping);
            _jumpedLastFrame = false;
            return;
        }

        if (!_isGrounded && _timeSinceLastGrounded > 0.15f)
        {
            _characterController.stepOffset = 0f;

            if (_verticalVelocity > 1f)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Jumping);
            }
            else if (_verticalVelocity < -1f)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Falling);
            }
        }
        else if (!_isGrounded && _verticalVelocity < -2f)
        {
            _characterController.stepOffset = 0f;
            _playerState.SetPlayerMoveState(PlayerMoveState.Falling);
        }
        else if (_isGrounded)
        {
            _characterController.stepOffset = _stepOffset;

            PlayerMoveState state;
            if (isSprinting)
            {
                state = PlayerMoveState.Sprinting;
            }
            else if (isMovingLaterlally || isMoving)
            {
                state = PlayerMoveState.Walking;
            }
            else
            {
                state = PlayerMoveState.Idle;
            }

            if (_playerInputs.Dance)
            {
                state = PlayerMoveState.Dancing;
            }

            _playerState.SetPlayerMoveState(state);
        }
    }

    private void GroundCheck()
    {
        /*float spherePositionY = transform.position.y + _characterController.radius - _characterController.skinWidth;
        Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);

        _isGrounded = Physics.CheckSphere(spherePosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);*/

        _groundShperePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _isGrounded = Physics.CheckSphere(_groundShperePos, _characterController.radius - 0.1f, _groundLayers, QueryTriggerInteraction.Ignore);

        Debug.Log("Is Grounded: " + _isGrounded);

        if (_isGrounded)
        {
            _timeSinceLastGrounded = 0f;
        }
        else
        {
            _timeSinceLastGrounded += Time.deltaTime;
        }
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
            _jumpedLastFrame = true;
        }
    }

    private void Movement()
    {
        Vector3 cameraForward = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraForward * _playerInputs.Move.y + cameraRight * _playerInputs.Move.x;

        float currentSpeed = _moveSpeed;
        if (_playerInputs.Aim)
        {
            currentSpeed = _aimSpeed;
        }
        else if (_playerInputs.Sprint)
        {
            currentSpeed = _sprintSpeed;
        }

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

    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

        return lateralVelocity.magnitude > 0.01f;
    }

    private void OnDrawGizmos()
    {
        _groundShperePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_groundShperePos, _characterController.radius - 0.1f);
    }

    public void SetOnLoad(Quaternion rotation)
    {
        _verticalVelocity = 0f;
        _timeSinceLastGrounded = 0f;

        _cameraRotation.x = rotation.eulerAngles.y;
        _cameraRotation.y = 0f;
        _playerTargetRotation.x = rotation.eulerAngles.y;
    }
}