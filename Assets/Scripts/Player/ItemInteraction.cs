using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [Header("Raycast params")] [SerializeField]
    private Transform origin;

    [SerializeField] private Transform direction;
    [SerializeField] private float reachDistance;
    [SerializeField] private LayerMask interactionLayer;

    [Header("Others")] [SerializeField] private GameObject interactableDisplay;
    [SerializeField] private AudioSource itemInteractionSound;

    private InteractableItem item;

    public static ItemInteraction Instance { get; private set; }


    public AudioSource ItemInteractionSound => itemInteractionSound;
    
    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        if (Physics.Raycast(origin.position, direction.forward, out RaycastHit hit, reachDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent(out item))
            {
                interactableDisplay.SetActive(true);
            }
        }
        else
        {
            item = null;
            interactableDisplay.SetActive(false);
        }
    }

    private void OnInteract()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        item?.Interact();
    }
}