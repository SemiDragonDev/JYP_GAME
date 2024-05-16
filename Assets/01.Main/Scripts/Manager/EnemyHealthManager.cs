using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : Singleton<EnemyHealthManager>
{
    private Animator anim;
    private string isDeadTag = "isDead";

    public void ManageHealth(FieldEnemy fieldEnemy)
    {
        anim = fieldEnemy.GetComponent<Animator>();

        if (fieldEnemy.Hp < 0)
        {
            anim.SetBool(isDeadTag, true);
        }
    }
}
