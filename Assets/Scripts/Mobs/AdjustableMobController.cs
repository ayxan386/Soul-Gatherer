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
    [Header("Player detection")] [SerializeField]
    private float playerSeeDistance;

    [SerializeField] private float playerReachDistance;
    [SerializeField] private LayerMask playerDetectionLayer;
    [SerializeField] private LayerMask playerOnlyLayer;


    [Header("Patrol Params")] [SerializeField]
    private Transform centerPoint;

    [SerializeField] private float patrollingDuration;
    [SerializeField] private float patrollingSpeed;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float defaultSpeed;

    [SerializeField] private float maxWanderDistance;

    [SerializeField] private List<StateActions> behaviors;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isHoldingWeapon;

    private Vector3 lastSelectedPatrolPoint = Vector3.zero;
    private MobStates currentState = MobStates.Idle;
    private NavMeshAgent agent;
    private Vector3 lastSawPosition;
    private float patrolStart;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (isHoldingWeapon)
        {
            animator.SetLayerWeight(1, 1);
        }
    }

    private void Update()
    {
        CheckIfCanSeePlayer();
        CheckIfCanReachPlayer();
        ApplyActionForCurrentState();
    }

    private void CheckIfCanSeePlayer()
    {
        var entInVision = Physics.OverlapSphere(transform.position, playerSeeDistance, playerDetectionLayer);
        currentState = MobStates.Idle;
        foreach (var collider in entInVision)
        {
            if (collider.CompareTag("Player"))
            {
                currentState = MobStates.CanSeePlayer;
                lastSawPosition = collider.transform.position;
                break;
            }
        }
    }

    private void CheckIfCanReachPlayer()
    {
        var entInReach = Physics.OverlapSphere(transform.position, playerReachDistance, playerOnlyLayer);
        if (currentState == MobStates.CanSeePlayer && entInReach.Length > 0)
        {
            currentState = MobStates.CanReachPlayer;
        }
    }

    private void ApplyActionForCurrentState()
    {
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
            || Vector3.Distance(transform.position, lastSelectedPatrolPoint) < 1f
            || patrolStart + patrollingDuration <= Time.time)
        {
            patrolStart = Time.time;
            lastSelectedPatrolPoint = centerPoint.position +
                                      new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * maxWanderDistance;
        }

        if (agent.enabled)
            agent.destination = lastSelectedPatrolPoint;
    }

    public void ChasePlayer()
    {
        if (agent.enabled)
            agent.destination = lastSawPosition;
    }

    public void SetSpeed(string speedName)
    {
        animator.SetBool("walking", true);
        animator.SetBool("running", false);
        if (speedName == "Chase")
        {
            agent.speed = chasingSpeed;
            animator.SetBool("running", true);
        }
        else if (speedName == "Patrol")
        {
            agent.speed = patrollingSpeed;
        }
        else
        {
            agent.speed = defaultSpeed;
        }
    }

    public void SetDead()
    {
        animator.SetBool("dead", true);
        // transform.Translate(0, -1, 0);
    }

    private void OnDrawGizmosSelected()
    {
        if (centerPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(centerPoint.position, maxWanderDistance);
        }
    }

    public void SetCenterPoint(Transform point)
    {
        centerPoint = point;
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