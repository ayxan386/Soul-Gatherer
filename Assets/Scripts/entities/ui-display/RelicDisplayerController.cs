using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicDisplayerController : MonoBehaviour
{
    [SerializeField] private RelicDisplayer displayerPrefab;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private List<RelicDisplayer> relicDisplayers = new List<RelicDisplayer>();

    void OnEnable()
    {
        UpdateUi();
        EventStore.Instance.OnRelicInventoryUpdate += OnRelicInventoryUpdate;
    }

    private void OnRelicInventoryUpdate()
    {
        UpdateUi();
    }

    private void UpdateUi()
    {
        while (transform.childCount < RelicInventoryController.Instance.InventorySize)
        {
            relicDisplayers.Add(Instantiate(displayerPrefab, transform));
        }

        for (int i = 0; i < relicDisplayers.Count; i++)
        {
            relicDisplayers[i].gameObject.SetActive(false);
        }

        int index = 0;
        foreach (var ownedRelic in RelicInventoryController.Instance.OwnedRelics)
        {
            relicDisplayers[index].gameObject.SetActive(true);
            relicDisplayers[index++].UpdateUi(ownedRelic, descriptionText);
        }
    }

    void OnDisable()
    {
        EventStore.Instance.OnRelicInventoryUpdate -= OnRelicInventoryUpdate;
    }
}