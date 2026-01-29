using DG.Tweening;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 _rotationVector = new Vector3(0, 360f, 0);
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower = 2f;
    [SerializeField] private float _jumpDuration = 4f;

    private void Start()
    {
        transform.DORotate(_rotationVector, _rotationSpeed, RotateMode.WorldAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        transform.DOJump(transform.position, _jumpPower / 10, 1, _jumpDuration).SetLoops(-1).SetEase(Ease.Linear);
    }
}
