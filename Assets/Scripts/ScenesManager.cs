using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string _mainSceneName = "MainScene";
    [SerializeField] private string _lampSceneName = "LampScene";

    [Header("Scene Loader Settings")]
    [SerializeField] private string _sceneLoaderName = "Portal";
    [SerializeField] private int _exitIndex = 2;

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

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        GameManager.Instance.EnableVisualPlayer();
    }
}
