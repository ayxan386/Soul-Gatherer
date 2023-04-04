using UnityEngine;

public class IdAssigner : MonoBehaviour
{
    [SerializeField] private int lastId;

    [ContextMenu("Assign ids")]
    public void AssignIds()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in allObjects)
        {
            if (obj is ILoadableEntity && !obj.TryGetComponent(out StringIdHolder idHolder))
            {
                var id = "" + lastId++;
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