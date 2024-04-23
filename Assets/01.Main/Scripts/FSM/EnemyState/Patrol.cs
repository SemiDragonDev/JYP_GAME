using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : FSMSingleton<Patrol>, IFSMState<EnemyStateManager>
{
    private IEnumerator coroutine;

    public void OnEnter(EnemyStateManager e)
    {
        if (e.knockBack.isAgentAvailable)
        {
            Debug.Log("Enter Patrol");
            e.InitPatrolDestination();
            e.SetTargetToPlayer();
        }
    }

    public void OnUpdate(EnemyStateManager e)
    {
        if (e.knockBack.isAgentAvailable)
        {
            e.UpdateDestination();
            if (!e.CheckPlayerIsDead())
            {
                if (e.CanSeePlayer() || e.IsCloseToTarget(e.targetToChase.position, e.minimumSensibleDist))
                {
                    e.ChangeState(Chase.Instance);
                }
            }
        }
    }

    public void OnExit(EnemyStateManager e)
    {
        Debug.Log("Stop Patrolling");
    }
}
