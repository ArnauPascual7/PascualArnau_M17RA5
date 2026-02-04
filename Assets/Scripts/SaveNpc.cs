using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SaveNpc : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    [SerializeField] private string _prompt = "Prem \"E\" per interactuar";
    [SerializeField] private GameObject _interactionCanvas;

    [Header("Text Messages")]
    [SerializeField] private TextMeshProUGUI _uiText;
    [SerializeField] private string _msgSave = "El joc s'ha guardat";
    [SerializeField] private string _msgLoad = "El joc s'ha carregat";
    [SerializeField] private string _msgEquip = "S'ha canviat l'estat del objecte equipat";

    public string InteractionPrompt => _prompt;

    [Header("Interact Events")]
    [Tooltip("Player Inputs Disable/Player Controller Cursor True")] public UnityEvent OnInteract;
    [Tooltip("Player Inputs Enable/Player Controller Cursor False")] public UnityEvent OnExitInteract;

    public bool Interact(PlayerInteraction player)
    {
        _uiText.text = string.Empty;

        OnInteract.Invoke();
        _interactionCanvas.SetActive(true);

        return true;
    }

    public void SaveGame()
    {
        _uiText.text = _msgSave;
        SaveSystem.Save();
    }

    public void LoadGame()
    {
        _uiText.text = _msgLoad;
        SaveSystem.Load();
    }

    public void EquipDesequipObj()
    {
        _uiText.text = _msgEquip;
        PlayerController.Instance.objCollected = !PlayerController.Instance.objCollected;
        PlayerController.Instance.EquipPartyHat(PlayerController.Instance.objCollected);
    }

    public void ExitInteraction()
    {
        OnExitInteract.Invoke();
        _interactionCanvas.SetActive(false);
    }
}
