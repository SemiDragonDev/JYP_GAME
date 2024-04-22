using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : FSMSingleton<Attack>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {
        e.SetTargetToPlayer();
        Debug.Log("Enter Attack");
        e.StopAnimBool("Move");
        e.PlayAnimTrigger("Attack");
    }

    public void OnUpdate(EnemyStateManager e)
    {

        // 거리가 1보다 멀어지면 >> CHASE
        if (!e.IsCloseToTarget(e.targetToChase.position, e.stopDistanceFromPlayer) && e.canMove)
        {
            e.ChangeState(Chase.Instance);
        }
        //5보다 멀때 >> Patrol
        else if (!e.IsCloseToTarget(e.targetToChase.position, e.minimumSensibleDist))
        {
            Debug.Log("Lost Player. Enter Patrol Again.");
            e.ChangeState(Patrol.Instance);
        }
        // When In Attack Range >> Attack
        else if (e.IsCloseToTarget(e.targetToChase.position, e.stopDistanceFromPlayer))
        {
            e.AttackWithDelay(2f);
            if (e.CheckPlayerIsDead()) { e.ChangeState(Patrol.Instance); }
        }
    }

    public void OnExit(EnemyStateManager e)
    {
        Debug.Log("Stop Attacking");
        e.MakeTargetNull();
    }
}
