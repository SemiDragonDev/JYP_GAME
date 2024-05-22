using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : FSMSingleton<Idle>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {

    }

    public void OnUpdate(EnemyStateManager e)
    {
        if (this.gameObject.activeSelf)
        {
            e.ChangeState(Patrol.Instance);
        }
    }

    public void OnExit(EnemyStateManager e)
    {
    }

}
