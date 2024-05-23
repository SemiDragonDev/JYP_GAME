using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour, IDamagable
{
    private WaitForSeconds waitForSeconds;

    public virtual float MaxHp { get; private set; }
    public virtual float Hp { get; set; }
    public virtual float AttackDamage { get;  }
    public virtual string Name {  get; private set; }
    public Enemy(string name)
    {
        Name = name;
    }

    public virtual void GetDamage(float damage)
    {
        Hp -= damage;
    }


    // 여기 수정중
    public virtual IEnumerator GetDamageOverTime(float damage, float interval, float lastingTime)
    {
        waitForSeconds = new WaitForSeconds(interval);
        float timeStacked = 0f;
        timeStacked += Time.deltaTime;
        while (timeStacked > lastingTime)
        {
            yield return waitForSeconds;
            GetDamage(damage);
        }
        timeStacked = 0f;
    }
}
