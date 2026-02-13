using UnityEngine;

[RequireComponent(typeof(MoveBehaviour))]
public class WalkBackwardsBehaviour : MonoBehaviour
{
    [SerializeField] private float _walkBackwardsSpeed = 2f;

    private MoveBehaviour _mb;
    private SprintBehaviour _sb;

    private void Awake()
    {
        _mb = GetComponent<MoveBehaviour>();
        _sb = GetComponent<SprintBehaviour>();
    }

    public bool IsWalkingBackwards(float y)
    {
        return y < -0.1f;
    }

    public void WalkBackwards()
    {
        _mb.MoveCharacter(_walkBackwardsSpeed);
    }
}
