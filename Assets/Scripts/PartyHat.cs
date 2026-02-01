using UnityEngine;

public class PartyHat : CollectibleObject
{
    public override bool Interact(PlayerInteraction player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller == null) return false;

        controller.objCollected = true;
        controller.EquipPartyHat(true);

        gameObject.SetActive(false);

        return true;
    }
}
