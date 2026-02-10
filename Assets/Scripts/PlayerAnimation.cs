using UnityEngine;
[RequireComponent(typeof(Animator), typeof(PlayerInputs), typeof(PlayerState))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private float _animationTransitionSpeed = 10f;
    public string VelocityParam = "Velocity";
    public string GroundedParam = "Grounded";
    public string JumpingParam = "Jumping";
    public string FallingParam = "Falling";
    public string DancingParam = "Dancing";
    public string HasRifleParam = "HasRifle";
    public string AimingParam = "Aiming";
    public string FireingParam = "Fireing";

    private Animator _animator;
    private PlayerInputs _playerInputs;
    private PlayerState _playerState;

    private float _currentBlendValue = 0f;

    private float _backwardsMaxBlendValue = -1f;
    private float _walkMaxBlendValue = 0.5f;
    private float _sprintMaxBlendValue = 1.5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInputs = GetComponent<PlayerInputs>();
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        bool isIdleing = _playerState.CurrentPlayerMoveState == PlayerMoveState.Idle;
        bool isWalking = _playerState.CurrentPlayerMoveState == PlayerMoveState.Walking;
        bool isSprinting = _playerState.CurrentPlayerMoveState == PlayerMoveState.Sprinting;
        bool isJumping = _playerState.CurrentPlayerMoveState == PlayerMoveState.Jumping;
        bool isFalling = _playerState.CurrentPlayerMoveState == PlayerMoveState.Falling;
        bool isGrounded = _playerState.InGroundState();
        bool isDancing = _playerState.CurrentPlayerMoveState == PlayerMoveState.Dancing;
        bool hasRifle = _playerState.CurrentPlayerAimState != PlayerAimState.None;
        bool isAiming = _playerState.CurrentPlayerAimState == PlayerAimState.Aiming || _playerState.CurrentPlayerAimState == PlayerAimState.Fireing;
        bool isFireing = _playerState.CurrentPlayerAimState == PlayerAimState.Fireing;

        // Calcular blend value amb direcció
        float targetBlendValue = 0f;

        if (_playerInputs.Move.magnitude > 0.01f)
        {
            // Detectar si es mou cap enrere
            bool isMovingBackwards = _playerInputs.Move.y < -0.1f;

            if (isMovingBackwards)
            {
                // Cap enrere: -1 a -0.5 segons la magnitud
                targetBlendValue = Mathf.Lerp(-0.5f, _backwardsMaxBlendValue, _playerInputs.Move.magnitude);
            }
            else if (isSprinting)
            {
                // Sprint: 1 fins a 1.5 segons la magnitud
                targetBlendValue = Mathf.Lerp(1f, _sprintMaxBlendValue, _playerInputs.Move.magnitude);
            }
            else
            {
                // Caminant: 0 a 1 segons la magnitud
                targetBlendValue = _playerInputs.Move.magnitude;
            }
        }

        _currentBlendValue = Mathf.Lerp(_currentBlendValue, targetBlendValue, _animationTransitionSpeed * Time.deltaTime);

        _animator.SetFloat(VelocityParam, _currentBlendValue);
        _animator.SetBool(GroundedParam, isGrounded);
        _animator.SetBool(JumpingParam, isJumping);
        _animator.SetBool(FallingParam, isFalling);
        _animator.SetBool(DancingParam, isDancing);
        _animator.SetBool(HasRifleParam, hasRifle);
        _animator.SetBool(AimingParam, isAiming);
        _animator.SetBool(FireingParam, isFireing);

        if (isDancing) _animator.SetLayerWeight(1, 0f);
        else _animator.SetLayerWeight(1, 1f);
    }
}