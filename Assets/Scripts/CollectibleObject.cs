using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Prem \"E\" per recollir";
    public string InteractionPrompt => _prompt;

    public bool Interact(PlayerInteraction player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller == null) return false;

        gameObject.SetActive(false);

        return true;
    }
}
