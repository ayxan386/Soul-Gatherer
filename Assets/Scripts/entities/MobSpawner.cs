using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] mobPrefabs;
    [SerializeField] private Vector2Int numberOfMobs;
    [SerializeField] private float distance;
    [SerializeField] private float size;
    [SerializeField] private float stepSize;
    [SerializeField] private LayerMask forbiddenLayers;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        var allPoints = GenerateAllPoints();
        Gizmos.color = Color.red;

        foreach (var point in allPoints)
        {
            Gizmos.DrawSphere(point, size);
        }
    }

    private Vector3[] GenerateAllPoints()
    {
        var res = new List<Vector3>();
        for (float x = -distance / 2; x < distance / 2; x += stepSize)
        {
            for (float z = -distance / 2; z < distance / 2; z += stepSize)
            {
                var pos = transform.position + new Vector3(x, 0, z);
                if (!Physics.CheckSphere(pos, size, forbiddenLayers))
                {
                    res.Add(pos);
                }
            }
        }

        return res.ToArray();
    }
}