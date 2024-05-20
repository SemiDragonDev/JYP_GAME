using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : FSMSingleton<Patrol>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {
        //Debug.Log("Enter Patrol");
        e.InitPatrolDestination();
        e.SetTargetToPlayer();
    }

    public void OnUpdate(EnemyStateManager e)
    {
        e.UpdateDestination();
        if (!e.CheckPlayerIsDead())
        {
            if (e.CanSeePlayer() || e.IsCloseToTarget(e.targetToChase.position, e.minimumSensibleDist))
            {
                e.ChangeState(Chase.Instance);
            }
        }

        if (e.IsDead())
        {
            e.ChangeState(Die.Instance);
        }
    }

    public void OnExit(EnemyStateManager e)
    {
        //Debug.Log("Stop Patrolling");
    }
}
