using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private float maxHp = 100f;
    private float currentHp;
    public override float MaxHp { get { return maxHp; } }
    public override float Hp { get { return currentHp; } set { currentHp = value; } }
    public override float AttackDamage { get { return 40f; } }

    public Skeleton() : base("Skeleton") { }

    private void Awake()
    {
        this.Hp = maxHp;
    }

    public override void GetDamage(float damage)
    {
        this.Hp -= damage;
    }
}
