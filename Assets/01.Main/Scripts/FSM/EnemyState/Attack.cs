using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : FSMSingleton<Attack>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {
        e.SetTargetToPlayer();
        //Debug.Log("Enter Attack");
        e.StopAnimBool("Move");
        e.PlayAnimTrigger("Attack");
    }

    public void OnUpdate(EnemyStateManager e)
    {
        if (!e.IsCloseToTarget(e.targetToChase.position, e.stopDistanceFromPlayer) && e.canMove)
        {
            e.ChangeState(Chase.Instance);
        }
        else if (!e.IsCloseToTarget(e.targetToChase.position, e.minimumSensibleDist))
        {
            Debug.Log("Lost Player. Enter Patrol Again.");
            e.ChangeState(Patrol.Instance);
        }
        else if (e.IsCloseToTarget(e.targetToChase.position, e.stopDistanceFromPlayer))
        {
            e.AttackWithDelay(2f);
            if (e.CheckPlayerIsDead()) { e.ChangeState(Patrol.Instance); }
        }

        // Attack 상태 진입 후 넉백이 일어나면 canMove가 False 상태로 끊어져서 Idle 상태로 멈추는 현상 발생
        // 넉백 밖 범위이고 Idle 인 경우 canMove를 true로 전환해 주는 코드
        if(!e.IsCloseToTarget(e.targetToChase.position, 1.3f) && e.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            e.canMove = true;
        }

        if(e.IsDead())
        {
            e.ChangeState(Die.Instance);
        }
    }

    public void OnExit(EnemyStateManager e)
    {
        //Debug.Log("Stop Attacking");
        e.MakeTargetNull();
    }
}
