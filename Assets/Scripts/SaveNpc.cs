using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SaveNpc : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    [SerializeField] private string _prompt = "Prem \"E\" per interactuar";

    [Header("Text Messages")]
    [SerializeField] private string _msgSave = "El joc s'ha guardat";
    [SerializeField] private string _msgLoad = "El joc s'ha carregat";
    [SerializeField] private string _msgEquip = "S'ha canviat l'estat del objecte equipat";

    public string InteractionPrompt => _prompt;

    public bool Interact(PlayerInteraction player)
    {
        UIManager.Instance.MenuSaveNPC = true;
        UIManager.Instance.MenuSaveNPCStateText = string.Empty;

        GameManager.Instance.SetPlayerInputsState(false);
        GameManager.Instance.SetCursorState(true);

        return true;
    }

    public void SaveGame()
    {
        UIManager.Instance.MenuSaveNPCStateText = _msgSave;
        SaveSystem.Save();
    }

    public void LoadGame()
    {
        UIManager.Instance.MenuSaveNPCStateText = _msgLoad;
        SaveSystem.Load();
    }

    public void EquipDesequipObj()
    {
        UIManager.Instance.MenuSaveNPCStateText = _msgEquip;
        GameManager.Instance.FlipPlayerPartyHatState();
    }

    public void ExitInteraction()
    {
        UIManager.Instance.MenuSaveNPCStateText = string.Empty;
        UIManager.Instance.MenuSaveNPC = false;

        GameManager.Instance.SetPlayerInputsState(true);
        GameManager.Instance.SetCursorState(false);
    }
}
