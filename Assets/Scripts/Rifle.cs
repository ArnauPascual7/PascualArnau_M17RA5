using UnityEngine;

public class Rifle : CollectibleObject
{
    public override bool Interact(PlayerInteraction player)
    {
        PlayerInventory pInventory = player.GetComponent<PlayerInventory>();

        if (pInventory == null) return false;

        pInventory.SetRifleState(true);

        base.Interact(player);

        return true;
    }
}
