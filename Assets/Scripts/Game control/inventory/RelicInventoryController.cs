using System.Collections.Generic;
using UnityEngine;

public class RelicInventoryController : MonoBehaviour
{
    [SerializeField] private List<BaseRelic> ownedRelics;

    public static RelicInventoryController Instance { get; private set; }

    public List<BaseRelic> OwnedRelics => ownedRelics;

    private void Awake()
    {
        Instance = this;
        ownedRelics = new List<BaseRelic>();
    }

    private void Start()
    {
        EventStore.Instance.OnRelicObtained += OnRelicObtained;
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnRelicObtained -= OnRelicObtained;
    }

    private void OnRelicObtained(BaseRelic newRelic)
    {
        if (newRelic.CanHaveMultiple)
        {
            AddRelicToInventory(newRelic);
        }
        else
        {
            var index = ownedRelics.FindIndex(relic => relic.Name == newRelic.Name);
            if (index < 0)
            {
                AddRelicToInventory(newRelic);
            }
            else
            {
                Destroy(newRelic.gameObject);
            }
        }
    }

    private void AddRelicToInventory(BaseRelic obj)
    {
        ownedRelics.Add(obj);
        obj.RelicObtained();
        obj.transform.parent = transform;
    }
}