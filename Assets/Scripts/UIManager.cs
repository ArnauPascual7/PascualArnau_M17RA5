using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    #region Save NPC Menu Settings
    [Header("Save NPC Menu UI Elements")]
    [SerializeField] private TextMeshProUGUI _pInteractionText;
    [SerializeField] private GameObject _menuSaveNPC;
    [SerializeField] private TextMeshProUGUI _menuStateText;

    [Header("Save NPC Text Messages")]
    [SerializeField] private string _msgSave = "El joc s'ha guardat";
    [SerializeField] private string _msgLoad = "El joc s'ha carregat";
    [SerializeField] private string _msgEquip = "S'ha canviat l'estat del objecte equipat";
    #endregion

    [Header("Player Explosion Settings")]
    [SerializeField] private GameObject _playerExplosionScreen;
    [SerializeField] private TextMeshProUGUI _respawnTimeText;
    [SerializeField] private float _respawnTime = 5f;

    public string PlayerInteractionText
    {
        get { return _pInteractionText.text; }
        set
        {
            _pInteractionText.text = value;
        }
    }

    #region Save NPC Menu UI Elements Properties
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
    #endregion

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

    #region Save NPC Menu Button Methods
    public void SaveGame()
    {
        _menuStateText.text = _msgSave;
        SaveSystem.Save();
    }

    public void LoadGame()
    {
        _menuStateText.text = _msgLoad;
        SaveSystem.Load();
    }

    public void EquipDesequipObj()
    {
        _menuStateText.text = _msgEquip;
        GameManager.Instance.FlipPlayerPartyHatState();
    }

    public void ExitInteraction()
    {
        _menuStateText.text = string.Empty;
        MenuSaveNPC = false;

        GameManager.Instance.SetPlayerInputsState(true);
        GameManager.Instance.SetCursorState(false);
    }
    #endregion

    public void PlayerExplosion()
    {
        _playerExplosionScreen.SetActive(true);

        StartCoroutine(StartRespawnTime());
    }

    private IEnumerator StartRespawnTime()
    {
        for (float timer = _respawnTime; timer > 0; timer -= Time.deltaTime)
        {
            _respawnTimeText.text = Mathf.Ceil(timer).ToString();
            yield return null;
        }

        _playerExplosionScreen.SetActive(false);
        ScenesManager.Instance.ReloadScene();
    }
}
