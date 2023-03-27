using System.Linq;
using UnityEngine;

public class IdAssigner : MonoBehaviour
{
    [SerializeField] private int lastId;

    [ContextMenu("Assign ids")]
    public void AssignIds()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>().OfType<ILoadableEntity>();
        foreach (var obj in allObjects)
        {
            obj.SetId(lastId++);
        }
    }
}