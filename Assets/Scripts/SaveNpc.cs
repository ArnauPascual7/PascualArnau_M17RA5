using UnityEngine;
using UnityEngine.Events;

public class SaveNpc : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    [SerializeField] private string _prompt = "Prem \"E\" per interactuar";
    [SerializeField] private GameObject _interactionCanvas;

    public string InteractionPrompt => _prompt;

    [Header("Interact Events")]
    [Tooltip("Player Inputs Disable/Player Controller Cursor True")] public UnityEvent OnInteract;
    [Tooltip("Player Inputs Enable/Player Controller Cursor False")] public UnityEvent OnExitInteract;

    public bool Interact(PlayerInteraction player)
    {
        OnInteract.Invoke();
        _interactionCanvas.SetActive(true);

        return true;
    }

    public void SaveGame()
    {
        SaveSystem.Save();
    }

    public void LoadGame()
    {
        SaveSystem.Load();
    }

    public void EquipDesequipObj()
    {
        PlayerController.Instance.objCollected = !PlayerController.Instance.objCollected;
        PlayerController.Instance.EquipPartyHat(PlayerController.Instance.objCollected);
    }

    public void ExitInteraction()
    {
        OnExitInteract.Invoke();
        _interactionCanvas.SetActive(false);
    }
}
