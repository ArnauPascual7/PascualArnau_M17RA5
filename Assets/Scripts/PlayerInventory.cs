using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject _partyHat;
    [SerializeField] private GameObject _rifle;

    public bool PartyHatIsEquipped { get; private set; }
    public bool RifleIsEquipped { get; private set; }

    public void SetPartyHatState(bool state)
    {
        PartyHatIsEquipped = state;
        _partyHat.SetActive(state);
    }

    public void SetRifleState(bool state)
    {
        RifleIsEquipped = state;
        _rifle.SetActive(state);
    }
}
