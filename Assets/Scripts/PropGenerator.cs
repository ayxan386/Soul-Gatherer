using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PropGenerator : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private float height;
    [SerializeField] private LayerMask forbiddenLayers;
    [SerializeField] private GameObject[] prefab;
    [SerializeField] private Transform holder;
    [SerializeField] private int desiredNumber;
    [SerializeField] private float size;
    [SerializeField] private float step;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private bool draw;

    [SerializeField] private string randomSeed;
    [SerializeField] [Range(0, 1f)] private float selectionThreshold;
    [SerializeField] private Vector2 scaleFactor;
    [SerializeField] private bool randomRotationAroundY;
    private Vector3 origin;
    [SerializeField] private Vector3 position;
    [SerializeField] private List<Vector3> normals;

    [ContextMenu("Generate position")]
    public void GeneratePositions()
    {
        origin = position;
        origin.y += height;
        positions = new List<Vector3>();
        for (float currentX = -1; currentX < 1; currentX += step)
        {
            for (float currentY = -1; currentY < 0; currentY += step)
            {
                for (float currentZ = -1; currentZ < 1; currentZ += step)
                {
                    var dir = new Vector3(currentX, currentY, currentZ).normalized;
                    if (Physics.Raycast(origin, dir, out RaycastHit hit, 200, ground))
                    {
                        if (!Physics.CheckSphere(hit.point, size, forbiddenLayers))
                        {
                            positions.Add(hit.point);
                            normals.Add(hit.normal);
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
        var normalTemp = new List<Vector3>();
        var index = 0;
        foreach (var position in positions)
        {
            if (Random.value <= selectionThreshold)
            {
                temp.Add(position);
                normalTemp.Add(normals[index]);
            }

            index++;
        }

        normals = normalTemp;
        positions = temp;
    }

#if UNITY_EDITOR
    [ContextMenu("Place prefabs")]
    public void PlacePrefabs()
    {
        var index = 0;
        foreach (var position in positions)
        {
            var gameObject =
                PrefabUtility.InstantiatePrefab(prefab[Random.Range(0, prefab.Length)], holder) as GameObject;
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.LookRotation(normals[index]);
            if (randomRotationAroundY)
                gameObject.transform.Rotate(0, Random.Range(-90, 90), 0);
            gameObject.transform.localScale *= Random.Range(scaleFactor.x, scaleFactor.y);
            index++;
        }
    }
#endif

    private void OnDrawGizmosSelected()
    {
        if (!draw) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(origin, 1f);

        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos, size);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(pos, normals[i] * size * 1.3f);
        }
    }

}