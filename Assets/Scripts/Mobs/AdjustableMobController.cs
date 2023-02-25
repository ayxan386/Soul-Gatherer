using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AdjustableMobController : MonoBehaviour
{
    // [SerializeField] private Transform areaCenter;
    [Header("Player detection")] [SerializeField]
    private float playerSeeDistance;

    [SerializeField] private float playerReachDistance;
    [SerializeField] private LayerMask playerDetectionLayer;
    [SerializeField] private LayerMask playerOnlyLayer;


    [Header("Patrol Params")] [SerializeField]
    private Transform centerPoint;

    [SerializeField] private float maxWanderDistance;

    [SerializeField] private List<StateActions> behaviors;

    private Vector3 lastSelectedPatrolPoint = Vector3.zero;
    private MobStates currentState = MobStates.Idle;
    private NavMeshAgent agent;
    private Vector3 lastSawPosition;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var entInVision = Physics.OverlapSphere(transform.position, playerSeeDistance, playerDetectionLayer);
        currentState = MobStates.Idle;
        foreach (var collider in entInVision)
        {
            if (collider.CompareTag("Player"))
            {
                print("Can see player");
                currentState = MobStates.CanSeePlayer;
                lastSawPosition = collider.transform.position;
                break;
            }

        }

        var entInReach = Physics.OverlapSphere(transform.position, playerReachDistance, playerOnlyLayer);
        if (currentState == MobStates.CanSeePlayer && entInReach.Length > 0)
        {
            print("Can reach player");
            currentState = MobStates.CanReachPlayer;
        }

        foreach (var behavior in behaviors)
        {
            if (behavior.currentState == currentState)
            {
                foreach (var unityEvent in behavior.actions)
                {
                    unityEvent.Invoke();
                }

                break;
            }
        }
    }

    public void Patrol()
    {
        if (lastSelectedPatrolPoint == Vector3.zero
            || Vector3.Distance(transform.position, lastSelectedPatrolPoint) < 1f)
        {
            lastSelectedPatrolPoint = centerPoint.position +
                                      new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * maxWanderDistance;
        }

        agent.destination = lastSelectedPatrolPoint;
    }

    public void ChasePlayer()
    {
        agent.destination = lastSawPosition;
    }
}

public enum MobStates
{
    Idle,
    CanSeePlayer,
    CanReachPlayer
}

[Serializable]
public class StateActions
{
    public MobStates currentState;
    [FormerlySerializedAs("action")] public UnityEvent[] actions;
}