using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : FSMSingleton<Die>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {
        // Death Animation 실행 후 아이템 드롭과 오브젝트 풀에 넣는 과정을 EnemyStateManager에 만들어서 이곳에서 실행하게 만들자
        e.animator.SetBool("IsDead", true);
    }

    public void OnUpdate(EnemyStateManager e)
    {
        
    }

    public void OnExit(EnemyStateManager e)
    {
    }
}
