using UnityEngine;

public class PlayerExplode : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;

    private PlayerInputs _playerInputs;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        if (_playerInputs.Explode)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _explosion.Play();

        GameManager.Instance.DisableVisualPlayer();
        UIManager.Instance.PlayerExplosion();
    }
}
