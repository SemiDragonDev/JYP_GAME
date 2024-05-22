using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : FSMSingleton<Die>, IFSMState<EnemyStateManager>
{
    private float passedTime;

    public void OnEnter(EnemyStateManager e)
    {
        // Death Animation 실행 후 아이템 드롭과 오브젝트 풀에 넣는 과정을 EnemyStateManager에 만들어서 이곳에서 실행하게 만들자
        e.animator.SetBool("IsDead", true);
    }

    public void OnUpdate(EnemyStateManager e)
    {
        passedTime += Time.deltaTime;
        if (passedTime > 4f) e.ChangeState(Idle.Instance);
    }

    public void OnExit(EnemyStateManager e)
    {
        Debug.Log("Die 상태 끝!");
        passedTime = 0f;
        e.animator.SetBool("IsDead", false);
        ObjectPool.Instance.ReturnToPool(e.pooledObject);
    }
}
