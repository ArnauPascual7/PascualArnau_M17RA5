using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string _mainSceneName = "MainScene";
    [SerializeField] private string _lampSceneName = "LampScene";

    [Header("Player Settings")]
    [SerializeField] private int _playerLayer = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            if (SceneManager.GetActiveScene().name == _mainSceneName)
            {
                SceneManager.LoadScene(_lampSceneName);
            }
            else if (SceneManager.GetActiveScene().name == _lampSceneName)
            {
                SceneManager.LoadScene(_mainSceneName);
            }
            else
            {
                Debug.LogError("Error Comparing Scenes: " + SceneManager.GetActiveScene().name);
            }
        }
    }
}
