using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private float maxHp = 100f;
    private float currentHp;
    public override float Hp { set { Hp = currentHp; } }
    public override float AttackDamage { get { return 40f; } }

    public Skeleton() : base("Skeleton") { }

    private void Awake()
    {
        this.currentHp = maxHp;
    }

    public override void GetDamage(float damage)
    {
        this.currentHp -= damage;
    }
}
