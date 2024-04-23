using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : FSMSingleton<Chase>, IFSMState<EnemyStateManager>
{
    private IEnumerator coroutine;

    public void OnEnter(EnemyStateManager e)
    {
        if (e.knockBack.isAgentAvailable)
        {
            Debug.Log("Enter Chase");
            e.SetTargetToPlayer();
            e.PlayAnimBool("Move");
        }
    }
    

    public void OnUpdate(EnemyStateManager e)
    {
        if (e.knockBack.isAgentAvailable)
        {
            e.ChasePlayer();
            if (e.IsCloseToTarget(e.targetToChase.position, e.stopDistanceFromPlayer))
            {
                Debug.Log("Player is close to attack");
                e.ChangeState(Attack.Instance);
            }
            else if (!e.CanSeePlayer() && !e.IsCloseToTarget(e.targetToChase.position, e.minimumSensibleDist))
            {
                Debug.Log("Lost Player. Patrol Again.");
                e.ChangeState(Patrol.Instance);
            }
        }
    }

    public void OnExit(EnemyStateManager e)
    {
        Debug.Log("Stop Chasing");
        e.MakeTargetNull();
    }
}
