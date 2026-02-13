using UnityEngine;
using UnityEngine.InputSystem;

public class CamRotationBehaviour : MonoBehaviour
{
    public Camera Camera { get => _cam; private set => _cam = value; }

    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _camTarget;

    public float lookSense = 0.1f;
    public float lookLimitV = 70f;
    public float rotationSpeed = 10f;

    private Vector2 _camRotation = Vector2.zero;
    private Vector2 _targetRotation = Vector2.zero;

    public void RotateCamera(float x, float y)
    {
        _camRotation.x += x * lookSense;
        _camRotation.y -= y * lookSense;
        _camRotation.y = Mathf.Clamp(_camRotation.y, -lookLimitV, lookLimitV);

        _targetRotation.x += transform.eulerAngles.x + lookSense * x;

        _camTarget.rotation = Quaternion.Euler(_camRotation.y, _camRotation.x, 0f);

        Quaternion targetRotationX = Quaternion.Euler(0f, _targetRotation.x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, rotationSpeed * Time.deltaTime);
    }
}
