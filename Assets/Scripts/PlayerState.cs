using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public PlayerMoveState CurrentPlayerMoveState {  get; private set; } = PlayerMoveState.Idle;

    public void SetPlayerMoveState(PlayerMoveState newState)
    {
        CurrentPlayerMoveState = newState;
    }

    public bool InGroundState()
    {
        return IsGroundState(CurrentPlayerMoveState);
    }

    public bool IsGroundState(PlayerMoveState state)
    {
        return
            state == PlayerMoveState.Idle ||
            state == PlayerMoveState.Walking ||
            state == PlayerMoveState.Sprinting;
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