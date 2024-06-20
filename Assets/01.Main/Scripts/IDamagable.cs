using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage);
    IEnumerator TakeDamageOverTime(float damage, float interval);
}
