using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour, ILoadableEntity
{
    [SerializeField] private AdjustableMobController[] mobPrefabs;
    [SerializeField] private Vector2Int numberOfMobs;
    [SerializeField] private float distance;
    [SerializeField] private float size;
    [SerializeField] [Min(0.1f)] private float stepSize;
    [SerializeField] private LayerMask forbiddenLayers;
    [SerializeField] private bool draw;
    [SerializeField] private bool alreadySpawned;

    private StringIdHolder assignedId;

    private void Awake()
    {
        assignedId = GetComponent<StringIdHolder>();
    }

    void Start()
    {
        if (alreadySpawned) return;

        alreadySpawned = true;
        var toSpawn = Random.Range(numberOfMobs.x, numberOfMobs.y);
        var allPoints = GenerateAllPoints();
        for (int i = 0; i < toSpawn; i++)
        {
            var pos = allPoints[Random.Range(0, allPoints.Length)];
            var mobPrefab = mobPrefabs[Random.Range(0, mobPrefabs.Length)];
            Instantiate(mobPrefab, pos, Quaternion.identity, transform).SetCenterPoint(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!draw) return;

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

    public void SetId(string id)
    {
        print("Set id called");
        assignedId = gameObject.AddComponent<StringIdHolder>();
        assignedId.id = id;
    }

    public string GetId()
    {
        return assignedId.id + GetType().Name;
    }

    public void LoadData(LoadableEntityData data)
    {
        if (data != null)
            alreadySpawned = data.wasInteracted;
    }

    public LoadableEntityData GetData()
    {
        var res = new LoadableEntityData();
        res.attachedId = GetId();
        res.wasInteracted = alreadySpawned;
        return res;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}