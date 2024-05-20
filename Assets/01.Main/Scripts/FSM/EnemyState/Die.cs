using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : FSMSingleton<Die>, IFSMState<EnemyStateManager>
{
    public void OnEnter(EnemyStateManager e)
    {
        e.PlayAnimBool("IsDead");
    }

    public void OnUpdate(EnemyStateManager e)
    {
    }

    public void OnExit(EnemyStateManager e)
    {
    }
}
