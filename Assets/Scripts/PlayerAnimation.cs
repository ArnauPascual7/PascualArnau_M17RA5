using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInputs), typeof(PlayerState))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private float _animationTransitionSpeed = 10f;

    public string VelocityParam = "Velocity";
    public string GroundedParam = "Grounded";
    public string JumpingParam = "Jumping";
    public string FallingParam = "Falling";

    private Animator _animator;

    private PlayerInputs _playerInputs;
    private PlayerState _playerState;

    private Vector3 _currentBlendInput = Vector3.zero;

    private float _walkMaxBlendValue = 0.5f;
    private float _sprintMaxBlendValue = 1f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _playerInputs = GetComponent<PlayerInputs>();
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        bool isIdle = _playerState.CurrentPlayerMoveState == PlayerMoveState.Idle;
        bool isWalking = _playerState.CurrentPlayerMoveState == PlayerMoveState.Walking;
        bool isSprinting = _playerState.CurrentPlayerMoveState == PlayerMoveState.Sprinting;
        bool isJumping = _playerState.CurrentPlayerMoveState == PlayerMoveState.Jumping;
        bool isFalling = _playerState.CurrentPlayerMoveState == PlayerMoveState.Falling;
        bool isGrounded = _playerState.InGroundState();

        Vector2 inputTarget =
            isSprinting ? _playerInputs.Move * _sprintMaxBlendValue :
            _playerInputs.Move * _walkMaxBlendValue;

        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, _animationTransitionSpeed * Time.deltaTime);

        _animator.SetFloat(VelocityParam, _currentBlendInput.magnitude);

        _animator.SetBool(GroundedParam, isGrounded);
        _animator.SetBool(JumpingParam, isJumping);
        _animator.SetBool(FallingParam, isFalling);
    }
}
