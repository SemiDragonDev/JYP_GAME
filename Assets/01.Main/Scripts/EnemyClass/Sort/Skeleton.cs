using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private float hp;
    public override float Hp {  get { return 100; } set { Hp = hp; } }
    public override float AttackDamage { get { return 40f; } }

    public Skeleton() : base("Skeleton") { }

    public override void GetDamage(float damage)
    {
        hp -= damage;
    }
}
