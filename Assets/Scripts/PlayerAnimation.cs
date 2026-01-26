using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInputs), typeof(PlayerState))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private float _animationTransitionSpeed = 1f;

    public string VelocityParam = "Velocity";

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
        Vector2 inputTarget = 
            _playerInputs.Sprint ? _playerInputs.Move * _sprintMaxBlendValue :
            _playerInputs.Move * _walkMaxBlendValue;

        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, _animationTransitionSpeed * Time.deltaTime);

        _animator.SetFloat(VelocityParam, _currentBlendInput.magnitude);
    }
}
