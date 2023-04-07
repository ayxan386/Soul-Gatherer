using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainedItemDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject obtainedMenu;
    [SerializeField] private ObtainedItemDisplayer itemDisplayPrefab;
    [SerializeField] private Transform displayParent;

    private List<ObtainedItemDisplayer> currentDisplayers;
    private List<string> currentDisplayerIds;
    private string itemId;

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedDisplay += OnOnEntityObtainedDisplay;
        EventStore.Instance.OnEntityObtainedClick += OnOnEntityObtainedClick;
    }

    private void OnOnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        if (e.attachedId != itemId || string.IsNullOrEmpty(e.attachedId)) return;

        var findIndex = currentDisplayerIds.FindIndex((temp) => temp == e.data.id);
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
        if (!obtainedMenu.activeInHierarchy || !obtainedMenu.activeSelf || entity.attachedId != itemId)
        {
            obtainedMenu.SetActive(true);
            itemId = entity.attachedId;
            currentDisplayers = new List<ObtainedItemDisplayer>();
            currentDisplayerIds = new List<string>();
        }

        ObtainedItemDisplayer obtainedItemDisplayer = Instantiate(itemDisplayPrefab, displayParent);
        obtainedItemDisplayer.Display(entity);
        StartCoroutine(ObtainAfterDelay(entity));
        currentDisplayers.Add(obtainedItemDisplayer);
        currentDisplayerIds.Add(entity.data.id);
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