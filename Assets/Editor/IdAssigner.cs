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
            if (obj is ILoadableEntity)
            {
                obj.name += "id = " + lastId++;
            }
        }
    }
}