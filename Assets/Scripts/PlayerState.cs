using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public PlayerMoveState CurrentPlayerMoveState {  get; private set; } = PlayerMoveState.Idle;
    [field: SerializeField] public PlayerAimState CurrentPlayerAimState { get; private set; } = PlayerAimState.None;

    public void SetPlayerMoveState(PlayerMoveState newState)
    {
        CurrentPlayerMoveState = newState;
    }

    public void SetPlayerAimState(PlayerAimState newState)
    {
        CurrentPlayerAimState = newState;
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
            state == PlayerMoveState.Sprinting ||
            state == PlayerMoveState.Dancing;
    }
}

public enum PlayerMoveState
{
    Idle = 0,
    Walking = 1,
    Sprinting = 2,
    Jumping = 3,
    Falling = 4,
    Dancing = 5
}

public enum PlayerAimState
{
    None = -1,
    Hip = 0,
    Aiming = 1,
    Fireing = 2
}