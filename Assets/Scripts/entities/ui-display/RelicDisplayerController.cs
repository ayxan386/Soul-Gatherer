using UnityEngine;

public class RelicDisplayerController : MonoBehaviour
{
    [SerializeField] private RelicDisplayer displayerPrefab;

    void OnEnable()
    {
        RelicInventoryController.Instance.OwnedRelics
            .ForEach(relic => { Instantiate(displayerPrefab, transform).UpdateUi(relic); });
    }

    void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}