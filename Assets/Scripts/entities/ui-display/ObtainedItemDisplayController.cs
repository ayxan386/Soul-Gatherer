using System.Collections.Generic;
using UnityEngine;

public class ObtainedItemDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject obtainedMenu;
    [SerializeField] private ObtainedItemDisplayer itemDisplayPrefab;
    [SerializeField] private Transform displayParent;

    private List<ObtainedItemDisplayer> currentDisplayers;
    private List<string> currentDisplayerIds;
    private string attachedId;

    void OnEnable()
    {
        EventStore.Instance.OnEntityObtainedDisplay += OnOnEntityObtainedDisplay;
        EventStore.Instance.OnEntityObtainedClick += OnOnEntityObtainedClick;
    }

    private void OnOnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        if (e.attachedId != attachedId) return;
        
        var findIndex = currentDisplayerIds.FindIndex((temp) => temp == e.data.id);
        if (findIndex >= 0)
        {
            Destroy(currentDisplayers[findIndex].gameObject);
            currentDisplayers.RemoveAt(findIndex);
            currentDisplayerIds.RemoveAt(findIndex);
        }

        if (currentDisplayerIds.Count == 0)
        {
            CloseObtainedMenu();
        }
    }

    public void CloseObtainedMenu()
    {
        GlobalStateManager.Instance.RunningGame();
        obtainedMenu.SetActive(false);
    }

    private void OnOnEntityObtainedDisplay(object sender, ObtainedEntity entity)
    {
        if (!obtainedMenu.activeInHierarchy || !obtainedMenu.activeSelf || entity.attachedId != attachedId)
        {
            GlobalStateManager.Instance.PausedGame();
            obtainedMenu.SetActive(true);
            attachedId = entity.attachedId;
            currentDisplayers = new List<ObtainedItemDisplayer>();
            currentDisplayerIds = new List<string>();
            for (int i = 0; i < displayParent.childCount; i++)
            {
                Destroy(displayParent.GetChild(i).gameObject);
            }
        }

        ObtainedItemDisplayer obtainedItemDisplayer = Instantiate(itemDisplayPrefab, displayParent);
        obtainedItemDisplayer.Display(entity);
        currentDisplayers.Add(obtainedItemDisplayer);
        currentDisplayerIds.Add(entity.data.id);
    }

    private void OnDisable()
    {
        EventStore.Instance.OnEntityObtainedDisplay -= OnOnEntityObtainedDisplay;
        EventStore.Instance.OnEntityObtainedClick -= OnOnEntityObtainedClick;
    }
}