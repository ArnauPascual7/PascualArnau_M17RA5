using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string _mainSceneName = "MainScene";
    [SerializeField] private string _lampSceneName = "LampScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SwitchScene()
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
            Debug.LogError("Error Comparing Scenes! Current Scene: " + SceneManager.GetActiveScene().name);
        }
    }

    public void SwitchScene(Vector3 playerPosition, Quaternion playerRotation)
    {
        if (SceneManager.GetActiveScene().name == _mainSceneName)
        {
            SceneManager.LoadScene(_lampSceneName);
            GameManager.Instance.SetPlayerPositionRotation(playerPosition, playerRotation);
        }
        else if (SceneManager.GetActiveScene().name == _lampSceneName)
        {
            SceneManager.LoadScene(_mainSceneName);
            GameManager.Instance.SetPlayerPositionRotation(playerPosition, playerRotation);
        }
        else
        {
            Debug.LogError("Error Comparing Scenes! Current Scene: " + SceneManager.GetActiveScene().name);
        }
    }
}
