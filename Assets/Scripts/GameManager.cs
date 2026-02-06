using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;  }

    private static GameObject Player { get; set; }

    [SerializeField] private string _playerTag = "Player";

    private PlayerInputs _playerInputs;
    private PlayerController _playerController;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        bool exitAwake = false;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            exitAwake = true;
        }

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag(_playerTag);
            DontDestroyOnLoad(Player);
        }
        else
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(_playerTag);
            
            foreach (GameObject obj in objects)
            {
                if (obj != Player)
                {
                    Destroy(obj);
                }
            }
        }

        if (exitAwake) return;

        if (Player != null)
        {
            _playerInputs = Player.GetComponent<PlayerInputs>();
            _playerController = Player.GetComponent<PlayerController>();
            _playerInventory = Player.GetComponent<PlayerInventory>();
        }
        else
        {
            Debug.LogError("ERROR! No s'ha trobat cap GameObject amb el tag: " + _playerTag);
        }
    }

    public void SetCursorState(bool state)
    {
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetPlayerInputsState(bool state)
    {
        _playerInputs.enabled = state;
    }

    public void FlipPlayerPartyHatState()
    {
        bool state = _playerInventory.PartyHatIsEquipped;
        _playerInventory.SetPartyHatState(!state);
    }

    public void Save(ref PlayerSaveData data)
    {
        data.Position = Player.transform.position;
        data.Rotation = Player.transform.rotation;
        data.PartyHat = _playerInventory.PartyHatIsEquipped;
    }

    public void Load(PlayerSaveData data)
    {
        CharacterController chController = Player.GetComponent<CharacterController>();
        chController.enabled = false;

        Player.transform.SetPositionAndRotation(data.Position, data.Rotation);
        _playerController.SetOnLoad(data.Rotation);

        chController.enabled = true;

        _playerInventory.SetPartyHatState(data.PartyHat);
    }
}

[Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public bool PartyHat;
}