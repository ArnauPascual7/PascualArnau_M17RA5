using UnityEngine;

[RequireComponent(typeof(MoveBehaviour))]
public class SprintBehaviour : MonoBehaviour
{
    [SerializeField] private float _sprintSpeed = 8f;

    private MoveBehaviour _mb;

    private void Awake()
    {
        _mb = GetComponent<MoveBehaviour>();
    }

    public void Sprint()
    {
        _mb.MoveCharacter(_sprintSpeed);
    }
}
