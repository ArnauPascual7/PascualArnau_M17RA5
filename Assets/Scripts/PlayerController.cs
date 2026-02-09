using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(PlayerInputs), typeof(CharacterController), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CinemachineCamera _danceCamera;
    [SerializeField] private GameObject _partyHat;
    [SerializeField] private GameObject _rifle;
    [SerializeField] private MultiAimConstraint[] _multiAimConstraints;
    [SerializeField] private Transform _rifleAimTarget;
    [SerializeField] private TwoBoneIKConstraint _leftHandIKConstrain;

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
    [SerializeField] private float _aimCameraDistance = 1f;
    [SerializeField] private float _aimCameraSideOffset = 0.7f;
    [SerializeField] private float _aimTransitionSpeed = 10f;

    private PlayerInputs _playerInputs;
    private PlayerState _playerState;
    private PlayerInventory _playerInventory;
    private CinemachineThirdPersonFollow _thirdPersonFollow;

    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;

    private float _timeSinceLastGrounded = 0f;
    private bool _jumpedLastFrame = false;
    private float _normalCameraDistance;
    private float _normalCameraSideOffset;
    private float _verticalVelocity;
    private bool _isGrounded;
    private float _stepOffset;
    private Vector3 _groundShperePos;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _playerState = GetComponent<PlayerState>();
        _playerInventory = GetComponent<PlayerInventory>();
        _thirdPersonFollow = _playerCamera.GetComponent<CinemachineThirdPersonFollow>();

        if (_thirdPersonFollow != null)
        {
            _normalCameraDistance = _thirdPersonFollow.CameraDistance;
            _normalCameraSideOffset = _thirdPersonFollow.CameraSide;
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
        UpdateAimState();

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

            if (_playerInventory.RifleIsEquipped)
            {
                Aim();
                RifleLook();
            }
        }
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void UpdateMoveState()
    {
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

    private void UpdateAimState()
    {
        PlayerAimState state;

        if (_playerInventory.RifleIsEquipped)
        {
            if (_playerInputs.Aim && _playerInputs.Fire)
            {
                state = PlayerAimState.Fireing;
            }
            else if (_playerInputs.Aim)
            {
                state = PlayerAimState.Aiming;
            }
            else
            {
                state = PlayerAimState.Hip;
            }
        }
        else
        {
            state = PlayerAimState.None;
        }

        _playerState.SetPlayerAimState(state);
    }

    private void GroundCheck()
    {
        _groundShperePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _isGrounded = Physics.CheckSphere(_groundShperePos, _characterController.radius - 0.1f, _groundLayers, QueryTriggerInteraction.Ignore);

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
            float cameraSideOffset = _playerInputs.Aim ? _aimCameraSideOffset : _normalCameraSideOffset;

            _thirdPersonFollow.CameraDistance = Mathf.Lerp(_thirdPersonFollow.CameraDistance, cameraDistance, _aimTransitionSpeed * Time.deltaTime);
            _thirdPersonFollow.CameraSide = Mathf.Lerp(_thirdPersonFollow.CameraSide, cameraSideOffset, _aimTransitionSpeed * Time.deltaTime);
        }
    }

    private void RifleLook()
    {
        if (_playerInputs.Aim)
        {
            foreach (var constraint in _multiAimConstraints)
            {
                constraint.weight = 0.5f;
            }
            _leftHandIKConstrain.weight = 1f;

            Vector2 screenCentre = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                _rifleAimTarget.transform.position = hitInfo.point;
            }
            else
            {
                _rifleAimTarget.transform.position = ray.GetPoint(100f);
            }

        }
        else
        {
            foreach (var constraint in _multiAimConstraints)
            {
                constraint.weight = 0f;
            }
            _leftHandIKConstrain.weight = 0f;
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