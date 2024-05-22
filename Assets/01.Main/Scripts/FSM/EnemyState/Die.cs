using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : FSMSingleton<Die>, IFSMState<EnemyStateManager>
{
    private float passedTime;

    public void OnEnter(EnemyStateManager e)
    {
        // Death Animation ���� �� ������ ��Ӱ� ������Ʈ Ǯ�� �ִ� ������ EnemyStateManager�� ���� �̰����� �����ϰ� ������
        e.animator.SetBool("IsDead", true);
    }

    public void OnUpdate(EnemyStateManager e)
    {
        passedTime += Time.deltaTime;
        if (passedTime > 4f) e.ChangeState(Idle.Instance);
    }

    public void OnExit(EnemyStateManager e)
    {
        Debug.Log("Die ���� ��!");
        passedTime = 0f;
        e.animator.SetBool("IsDead", false);
        ObjectPool.Instance.ReturnToPool(e.pooledObject);
    }
}
