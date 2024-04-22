using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : FSM<EnemyStateManager>
{
    [Header("Waypoints / Patrol")]
    public float waitingTime = 5f;
    public float elapsedTime = 0;

    [Space(10)]
    [Header("Chase")]
    public GameObject player;
    public Transform targetToChase;
    public float stopDistanceFromPlayer = 1f;
    public float minimumSensibleDist = 5f;
    public float distanceOfView = 10f;
    [SerializeField]
    [Range(0f,360f)]
    private float angleOfView = 90f;
    [SerializeField]
    private LayerMask obstacleMask;

    [Space(10)]
    [Header("Attack")]
    private float passedTime = 0;
    public bool canMove;

    private float stillThreshold = 0.05f;
    private float maxKnockBackTime = 0.5f;

    private NavMeshAgent agent;
    private Rigidbody rigidBody;
    public Animator animator;
    public PlayerHealth playerHealth;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        InitState(this, Patrol.Instance);
    }

    private void Update()
    {
        FSMUpdate();
    }

    public Vector3 GetRandomPosition(float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir *= radius;
        randomDir += transform.position;
        NavMeshHit hit;
        Vector3 finalPos = Vector3.zero;
        if (NavMesh.SamplePosition(randomDir, out hit, radius, 1))
        {
            finalPos = hit.position;
        }
        return finalPos;
    }


    public void InitPatrolDestination()
    {
        agent.stoppingDistance = 0f;
        agent.speed = 1f;
        agent.SetDestination(GetRandomPosition(20f));
        PlayAnimBool("Move");
    }

    public void UpdateDestination()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StopAnimBool("Move");
            elapsedTime += Time.deltaTime;
            if (elapsedTime > waitingTime)
            {
                agent.SetDestination(GetRandomPosition(20f));
                elapsedTime = 0;
                PlayAnimBool("Move");
            }
        }
    }

    public bool CanSeePlayer()
    {
        SetTargetToPlayer();
        Vector3 dir = targetToChase.position - transform.position;

        float dot = Vector3.Dot(transform.forward, dir);

        float targetCos = Mathf.Cos(Mathf.Deg2Rad * (angleOfView / 2));
        float targetDist = dir.magnitude;

        if (targetCos < dot && targetDist < distanceOfView)
        {
            if (!Physics.Raycast(transform.position, dir, distanceOfView, obstacleMask))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public void ChasePlayer()
    {
        agent.stoppingDistance = stopDistanceFromPlayer;
        agent.speed = 1f;
        agent.SetDestination(targetToChase.position);
    }

    public bool IsCloseToTarget(Vector3 target, float distance)
    {
        var dir = target - transform.position;
        if (dir.magnitude <= distance) return true;
        else return false;
    }

    public void LookAtPlayer(Vector3 target)
    {
        this.transform.LookAt(target);
    }

    public void MakeTargetNull()
    {
        targetToChase = null;
    }

    public void SetTargetToPlayer()
    {
        targetToChase = player.transform;
    }

    public void PlayAnimBool(string anim)
    {
        animator.SetBool(anim, true);
    }

    public void PlayAnimTrigger(string anim)
    {
        animator.SetTrigger(anim);
    }

    public void StopAnimBool(string anim)
    {
        animator.SetBool(anim, false);
    }

    public void AttackWithDelay(float delay)
    {
        passedTime += Time.deltaTime;
        if (passedTime >= delay)
        {
            LookAtPlayer(targetToChase.position);
            Debug.Log("Attack!");
            PlayAnimTrigger("Attack");
            passedTime = 0;
        }
    }

    public void GiveDamage()
    {
        if (IsCloseToTarget(player.transform.position, stopDistanceFromPlayer))
        {
            playerHealth.TakeDamage(40, this.transform);
        }
    }

    public bool CheckPlayerIsDead()
    {
        if (playerHealth.isDead) return true;
        return false;
    }

    public void MoveAvailable()
    {
        canMove = true;
    }

    public void MoveUnavailable()
    {
        canMove = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
