using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;

    [Header("Settings")]
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private LayerMask _groundLayers;
    
    private PlayerInputs _playerInputs;

    private Vector3 _groundedSpherePosition;
    private float _groundedSphereRadius;
    private bool _isGrounded;

    private void Awake()
    {
        //_playerInputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        //_characterController.Move(new Vector3(0f, -(_gravity * Time.deltaTime), 0f));
    }

    private void GroundCheck()
    {
        _groundedSpherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);
        _groundedSphereRadius = _characterController.radius + _characterController.skinWidth;

        _isGrounded = Physics.CheckSphere(_groundedSpherePosition, _groundedSphereRadius, _groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_groundedSpherePosition, _groundedSphereRadius);
    }
}
