using Unity.Cinemachine;
using UnityEngine;

public class DanceBehaviour : MonoBehaviour
{
    public int DanceCameraPriority { get => _danceCamera.Priority; set { _danceCamera.Priority = value; } }

    [SerializeField] private CinemachineCamera _danceCamera;

    public bool IsDancing { get; set; } = false;
}
