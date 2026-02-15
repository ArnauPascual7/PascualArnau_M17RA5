using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string _mainSceneName = "MainScene";
    [SerializeField] private string _lampSceneName = "LampScene";

    [Header("Scene Loader Settings")]
    [SerializeField] private string _sceneLoaderName = "Portal";
    [SerializeField] private int _exitIndex = 2;

    private bool _isLoadingScene = false;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_isLoadingScene)
        {
            _isLoadingScene = false;
            StartCoroutine(PositionPlayerAfterLoad());
        }
    }

    private IEnumerator PositionPlayerAfterLoad()
    {
        yield return null;

        GameObject loader = GameObject.Find(_sceneLoaderName);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (loader != null && player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
            }

            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            Transform exitPoint = null;
            if (_exitIndex >= 0 && _exitIndex < loader.transform.childCount)
            {
                exitPoint = loader.transform.GetChild(_exitIndex);
            }

            if (exitPoint != null)
            {
                player.transform.position = exitPoint.position;
                player.transform.rotation = exitPoint.rotation;
            }
            else
            {
                Debug.LogWarning($"No s'ha pogut trobar el fill {_exitIndex} del loader. Total fills: {loader.transform.childCount}");
            }

            yield return null;
            yield return null;

            if (cc != null)
            {
                cc.enabled = true;
            }

            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
        else
        {
            if (loader == null) Debug.LogWarning($"No s'ha trobat el loader: {_sceneLoaderName}");
            if (player == null) Debug.LogWarning("No s'ha trobat el player amb tag 'Player'");
        }
    }

    public void SwitchScene()
    {
        if (_isLoadingScene) return;

        _isLoadingScene = true;

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
            _isLoadingScene = false;
            Debug.LogError("Error Comparing Scenes! Current Scene: " + SceneManager.GetActiveScene().name);
        }
    }

    public void ReloadScene()
    {
        _isLoadingScene = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.EnableVisualPlayer();
    }
}
