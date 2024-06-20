using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour, IDamagable
{
    public bool isTakingDamage = false;
    public virtual float MaxHp { get; private set; }
    public virtual float Hp { get; set; }
    public virtual float AttackDamage { get;  }
    public virtual string Name {  get; private set; }
    public Enemy(string name)
    {
        Name = name;
    }

    Renderer[] renderers;
    Color[] originalColors;

    DayNightCycle dayNightCycle;
    IEnumerator coroutine;

    private void OnEnable()
    {
        dayNightCycle = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DayNightCycle>();
        coroutine = TakeDamageOverTime(10f, 1f);
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }

    private void Update()
    {
        if (dayNightCycle.isDay && !isTakingDamage)
        {
            StartCoroutine(coroutine);
            PlayBurnEffect();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
    }

    public virtual IEnumerator TakeDamageOverTime(float damage, float interval)
    {
        isTakingDamage = true;
        while(Hp > 0)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(interval);
        }
        isTakingDamage = false;
    }

    public virtual void PlayBurnEffect()
    {
        this.gameObject.transform.Find("BurnEffect").gameObject.SetActive(true);
    }
}