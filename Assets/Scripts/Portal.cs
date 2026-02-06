using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;

    [Header("Player Settings")]
    [SerializeField] private int _playerLayer = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            ScenesManager.Instance.SwitchScene(_exitPoint.position, transform.rotation);
        }
    }
}
