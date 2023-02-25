using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AdjustableMobController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool followTarget;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (followTarget)
            agent.destination = target.position;
    }
}