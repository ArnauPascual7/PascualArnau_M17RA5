using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Prem \"E\" per recollir";
    public string InteractionPrompt => _prompt;

    public virtual bool Interact(PlayerInteraction player)
    {
        gameObject.SetActive(false);

        return true;
    }
}
