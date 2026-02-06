using UnityEngine;

public class PartyHat : CollectibleObject
{
    [Header("Parent Object")]
    [SerializeField] private GameObject _parent;

    public override bool Interact(PlayerInteraction player)
    {
        PlayerInventory pInventory = player.GetComponent<PlayerInventory>();

        if (pInventory == null) return false;

        pInventory.SetPartyHatState(true);

        _parent.SetActive(false);

        return true;
    }
}
