using UnityEngine;

[RequireComponent(typeof(GravityBehaviour), typeof(MoveBehaviour))]
public class JumpBehaviour : MonoBehaviour
{
    public bool JumpedLastFrame { get; set; } = false;

    [SerializeField] private float _jumpHeight = 10f;

    private GravityBehaviour _gb;
    private MoveBehaviour _mb;

    private void Awake()
    {
        _gb = GetComponent<GravityBehaviour>();
        _mb = GetComponent<MoveBehaviour>();
    }

    public void Jump()
    {
        if (_gb.isGrounded)
        {
            float verticalVelocity = Mathf.Sqrt((_jumpHeight / 10f) * 3f * _gb.gravity);

            _mb.SetVerticalVelocity(verticalVelocity);
            JumpedLastFrame = true;
        }
    }
}
