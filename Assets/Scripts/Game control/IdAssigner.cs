using System;
using UnityEngine;

public class IdAssigner : MonoBehaviour
{
    [SerializeField] private int lastId;
    public static IdAssigner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("Assign ids")]
    public void AssignIds()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in allObjects)
        {
            if (obj is ILoadableEntity && !obj.TryGetComponent(out StringIdHolder idHolder))
            {
                var id = Guid.NewGuid().ToString() + lastId++;
                (obj as ILoadableEntity).SetId(id);
                print(id);
            }
        }
    }

    [ContextMenu("Delete ids")]
    public void DeleteIds()
    {
        var allObjects = FindObjectsOfType<StringIdHolder>();
        foreach (var obj in allObjects)
        {
            DestroyImmediate(obj);
        }
    }
}