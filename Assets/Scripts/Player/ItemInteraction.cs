using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [Header("Raycast params")] [SerializeField]
    private Transform origin;

    [SerializeField] private Transform direction;
    [SerializeField] private float reachDistance;
    [SerializeField] private LayerMask interactionLayer;

    [Header("Others")] [SerializeField] private GameObject interactableDisplay;

    private InteractableItem item;


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