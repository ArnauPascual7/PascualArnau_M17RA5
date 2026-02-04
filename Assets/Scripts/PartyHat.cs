using UnityEngine;

public class PartyHat : CollectibleObject
{
    [Header("Parent Object")]
    [SerializeField] private GameObject _parent;

    public override bool Interact(PlayerInteraction player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller == null) return false;

        controller.objCollected = true;
        controller.EquipPartyHat(true);

        _parent.SetActive(false);

        return true;
    }
}
