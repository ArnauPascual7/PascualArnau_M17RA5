using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public PlayerMoveState CurrentPlayerMoveState {  get; private set; }

    public void SetPlayerMoveState(PlayerMoveState newState)
    {
        CurrentPlayerMoveState = newState;
    }
}

public enum PlayerMoveState
{
    Idle = 0,
    Walking = 1,
    Sprinting = 2,
    Jumping = 3,
    Falling = 4
}