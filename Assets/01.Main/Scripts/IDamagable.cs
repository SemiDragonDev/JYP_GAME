using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void GetDamage(float damage);
    IEnumerator GetDamageOverTime(float damage, float interval);
}
