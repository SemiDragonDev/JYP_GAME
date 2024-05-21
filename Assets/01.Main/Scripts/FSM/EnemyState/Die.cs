using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : FSMSingleton<Die>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {
        // Death Animation ���� �� ������ ��Ӱ� ������Ʈ Ǯ�� �ִ� ������ EnemyStateManager�� ���� �̰����� �����ϰ� ������
        e.animator.SetBool("IsDead", true);
    }

    public void OnUpdate(EnemyStateManager e)
    {
        
    }

    public void OnExit(EnemyStateManager e)
    {
    }
}
