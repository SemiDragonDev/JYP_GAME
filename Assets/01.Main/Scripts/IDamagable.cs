using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void GetDamage(float damage);
    IEnumerator GetDotDamage(float damage, float interval, float lastingTime);
}
