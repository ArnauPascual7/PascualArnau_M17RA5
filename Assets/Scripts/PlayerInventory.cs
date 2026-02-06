using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject _partyHat;

    public bool PartyHatIsEquipped { get; private set; }

    public void SetPartyHatState(bool state)
    {
        PartyHatIsEquipped = state;
        _partyHat.SetActive(state);
    }
}
