using Newtonsoft.Json.Bson;
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

    private Enemy enemyInfo;
    private NavMeshAgent agent;
    public Animator animator;
    public PooledObject pooledObject;
    public PlayerHealth playerHealth;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        InitState(this, Patrol.Instance);
        enemyInfo = GetComponent<Enemy>();
        pooledObject = GetComponent<PooledObject>();
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
            playerHealth.TakeDamage(40);
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
    
    // 체력을 확인하는 메서드를 만들자 => Die State를 만들어야 함
    public bool IsDead()
    {
        if (enemyInfo.Hp <= 0)
        {
            return true;
        }
        return false;
    }

    // 죽은 이후 다음 리스폰을 위해 HP를 다시 Max로 만들어주고, 낮에 활성화된 BurnEffect도 비활성화 시켜준다
    public void ResetStatesForRespawn()
    {
        enemyInfo.Hp = enemyInfo.MaxHp;
        this.transform.Find("BurnEffect").gameObject.SetActive(false);
    }
}
