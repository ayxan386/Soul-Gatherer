using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainedItemDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject obtainedMenu;
    [SerializeField] private ObtainedItemDisplayer itemDisplayPrefab;
    [SerializeField] private Transform displayParent;

    private List<ObtainedItemDisplayer> currentDisplayers;

    private List<Guid> currentDisplayerIds;
    // private string itemId;

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedDisplay += OnOnEntityObtainedDisplay;
        EventStore.Instance.OnEntityObtainedClick += OnOnEntityObtainedClick;
        currentDisplayers = new List<ObtainedItemDisplayer>();
        currentDisplayerIds = new List<Guid>();
    }

    private void OnOnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        var findIndex = currentDisplayerIds.FindIndex((temp) => temp == e.id);
        if (findIndex >= 0)
        {
            Destroy(currentDisplayers[findIndex].gameObject);
            currentDisplayers.RemoveAt(findIndex);
            currentDisplayerIds.RemoveAt(findIndex);
        }

        if (currentDisplayers.Count == 0)
        {
            obtainedMenu.SetActive(false);
        }
    }

    private void OnOnEntityObtainedDisplay(object sender, ObtainedEntity entity)
    {
        print("Display event received");
        if (!obtainedMenu.activeInHierarchy || !obtainedMenu.activeSelf)
        {
            obtainedMenu.SetActive(true);
        }

        ObtainedItemDisplayer obtainedItemDisplayer = Instantiate(itemDisplayPrefab, displayParent);
        currentDisplayers.Add(obtainedItemDisplayer);
        currentDisplayerIds.Add(entity.id);
        obtainedItemDisplayer.Display(entity);
        StartCoroutine(ObtainAfterDelay(entity));
    }

    private IEnumerator ObtainAfterDelay(ObtainedEntity obtainedEntity)
    {
        yield return new WaitForSeconds(1.2f);
        EventStore.Instance.PublishEntityObtainedClick(obtainedEntity);
    }

    private void OnDisable()
    {
        EventStore.Instance.OnEntityObtainedDisplay -= OnOnEntityObtainedDisplay;
        EventStore.Instance.OnEntityObtainedClick -= OnOnEntityObtainedClick;
    }
}