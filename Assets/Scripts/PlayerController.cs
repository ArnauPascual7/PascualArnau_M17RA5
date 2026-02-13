using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputs), typeof(MoveBehaviour), typeof(JumpBehaviour))]
[RequireComponent(typeof(GravityBehaviour), typeof(CamRotationBehaviour), typeof(SprintBehaviour))]
[RequireComponent(typeof(WalkBackwardsBehaviour), typeof(DanceBehaviour))]
[RequireComponent(typeof(PlayerState), typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    private PlayerInputs _playerInputs;
    private PlayerState _playerState;

    private MoveBehaviour _mb;
    private JumpBehaviour _jb;
    private GravityBehaviour _gb;
    private CamRotationBehaviour _crb;
    private SprintBehaviour _sb;
    private WalkBackwardsBehaviour _rb;
    private DanceBehaviour _db;

    private float _stepOffset;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _playerInputs = GetComponent<PlayerInputs>();
        _playerState = GetComponent<PlayerState>();

        _mb = GetComponent<MoveBehaviour>();
        _jb = GetComponent<JumpBehaviour>();
        _gb = GetComponent<GravityBehaviour>();
        _crb = GetComponent<CamRotationBehaviour>();
        _sb = GetComponent<SprintBehaviour>();
        _rb = GetComponent<WalkBackwardsBehaviour>();
        _db = GetComponent<DanceBehaviour>();

        _stepOffset = _characterController.stepOffset;
    }

    private void Start()
    {
        GameManager.Instance.SetCursorState(false);
    }

    private void Update()
    {
        Dance();

        UpdateMoveState();

        Gravity();

        if (_db.IsDancing)
        {
            _db.DanceCameraPriority = 1;
            GameManager.Instance.SetPlayerRifleState(false);
        }
        else
        {
            _db.DanceCameraPriority = 0;
            GameManager.Instance.SetPlayerRifleState(true);

            Jump();
            Move();
        }
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    private void Dance()
    {
        if (_playerInputs.Dance)
        {
            _db.IsDancing = !_db.IsDancing;
        }
    }

    private void UpdateMoveState()
    {
        bool isMoving = _playerInputs.Move != Vector2.zero;
        bool isMovingLaterlally = _mb.IsMovingLaterally();
        bool isMovingBackwards = _rb.IsWalkingBackwards(_playerInputs.Move.y);
        bool isSprinting = _playerInputs.Sprint && isMovingLaterlally && !isMovingBackwards;

        if (_jb.JumpedLastFrame)
        {
            _characterController.stepOffset = 0f;
            _playerState.SetPlayerMoveState(PlayerMoveState.Jumping);
            _jb.JumpedLastFrame = false;
            return;
        }

        if (!_gb.isGrounded && _gb.TimeSinceLastGrounded > 0.15f)
        {
            _characterController.stepOffset = 0f;

            if (_mb.Velocity.y > 1f)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Jumping);
            }
            else if (_mb.Velocity.y < -1f)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Falling);
            }
        }
        else if (!_gb.isGrounded && _mb.Velocity.y < -2f)
        {
            _characterController.stepOffset = 0f;
            _playerState.SetPlayerMoveState(PlayerMoveState.Falling);
        }
        else if (_gb.isGrounded)
        {
            _characterController.stepOffset = _stepOffset;

            if (isSprinting)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Sprinting);
            }
            else if (isMoving)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Walking);
            }
            else
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Idle);
            }

            if (_db.IsDancing)
            {
                _playerState.SetPlayerMoveState(PlayerMoveState.Dancing);
            }
        }
    }

    private void Gravity()
    {
        _gb.UpdateGravity();
    }

    private void Jump()
    {
        if (_playerInputs.Jump)
        {
            _jb.Jump();
        }
    }

    private void Move()
    {
        _mb.SetMoveDirection(_crb.Camera, _playerInputs.Move.x, _playerInputs.Move.y);

        if (_rb.IsWalkingBackwards(_playerInputs.Move.y))
        {
            _rb.WalkBackwards();
        }
        else
        {
            if (_playerInputs.Sprint)
            {
                _sb.Sprint();
            }
            else
            {
                _mb.MoveCharacter();
            }
        }
    }

    private void RotateCamera()
    {
        _crb.RotateCamera(_playerInputs.Look.x, _playerInputs.Look.y);
    }
}
