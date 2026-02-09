using UnityEngine;

public class SaveNpc : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    [SerializeField] private string _prompt = "Prem \"E\" per interactuar";

    public string InteractionPrompt => _prompt;

    public bool Interact(PlayerInteraction player)
    {
        UIManager.Instance.MenuSaveNPC = true;
        UIManager.Instance.MenuSaveNPCStateText = string.Empty;

        GameManager.Instance.SetPlayerInputsState(false);
        GameManager.Instance.SetCursorState(true);

        return true;
    }
}
