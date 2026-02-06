using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public string PlayerInteractionText
    {
        get { return _pInteractionText.text; }
        set
        {
            _pInteractionText.text = value;
        }
    }
    public bool MenuSaveNPC
    {
        get { return _menuSaveNPC.activeSelf; }
        set
        {
            _menuSaveNPC.SetActive(value);
        }
    }

    public string MenuSaveNPCStateText
    {
        get { return _menuStateText.text; }
        set
        {
            _menuStateText.text = value;
        }
    }

    [SerializeField] private TextMeshProUGUI _pInteractionText;
    [SerializeField] private GameObject _menuSaveNPC;
    [SerializeField] private TextMeshProUGUI _menuStateText;

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
}
