using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private TextMeshProUGUI _interactionText;

    private readonly Collider[] _colliders = new Collider[3];
    private int _numFound;

    private PlayerInputs _playerInputs;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        
        if (_numFound > 0)
        {
            IInteractable interactable = _colliders[0].GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                UIManager.Instance.PlayerInteractionText = interactable.InteractionPrompt;

                if (_playerInputs.Interact)
                {
                    bool success = interactable.Interact(this);
                    if (!success) Debug.LogError("Error on interacting with object: " + interactable.ToString());
                }
            }
            else
            {
                UIManager.Instance.PlayerInteractionText = string.Empty;
            }
        }
        else
        {
            UIManager.Instance.PlayerInteractionText = string.Empty;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
