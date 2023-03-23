using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropGenerator : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private float height;
    [SerializeField] private LayerMask forbiddenLayers;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform holder;
    [SerializeField] private int desiredNumber;
    [SerializeField] private float size;
    [SerializeField] private float step;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private bool draw;

    [SerializeField] private string randomSeed;
    [SerializeField] [Range(0, 1f)] private float selectionThreshold;
    [SerializeField] private Vector2 scaleFactor;
    private Vector3 origin;

    [ContextMenu("Generate position")]
    public void GeneratePositions()
    {
        origin = transform.position;
        origin.y += height;
        positions = new List<Vector3>();
        while (positions.Count < desiredNumber)
        {
            for (float currentX = -1; currentX < 1; currentX += step)
            {
                // var xVector = Vector3.Lerp(Vector3.left, Vector3.right, currentX);
                for (float currentY = -1; currentY < 0; currentY += step)
                {
                    // var yVector = Vector3.Lerp(Vector3.back, Vector3.forward, currentY);
                    for (float currentZ = -1; currentZ < 1; currentZ += step)
                    {
                        // var dir = Vector3.Lerp(xVector, yVector, currentZ);
                        var dir = new Vector3(currentX, currentY, currentZ).normalized;
                        if (Physics.Raycast(origin, dir, out RaycastHit hit, 200, ground))
                        {
                            if (!Physics.CheckSphere(hit.point, size, forbiddenLayers))
                            {
                                positions.Add(hit.point);
                            }
                        }
                    }
                }
            }
        }
    }

    [ContextMenu("Select randomly")]
    public void SelectRandomly()
    {
        Random.InitState(randomSeed.GetHashCode());
        var temp = new List<Vector3>();
        foreach (var position in positions)
        {
            if (Random.value <= selectionThreshold)
            {
                temp.Add(position);
            }
        }

        positions = temp;
    }

    [ContextMenu("Place prefabs")]
    public void PlacePrefabs()
    {
        foreach (var position in positions)
        {
            // PrefabUtils.
            var gameObject = PrefabUtility.InstantiatePrefab(prefab, holder) as GameObject;
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.Rotate(0, Random.Range(-90, 90), 0);
            gameObject.transform.localScale *= Random.Range(scaleFactor.x, scaleFactor.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!draw) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(origin, 1f);
        Gizmos.color = Color.green;

        foreach (var dir in positions)
        {
            Gizmos.DrawSphere(dir, size);
        }
    }
}