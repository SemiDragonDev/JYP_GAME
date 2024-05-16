using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : FieldEnemy, IDamagable
{
    private float hp;
    public override float Hp { get { return 100f; } set { Hp = hp; } }
    public override float AttackDamage { get { return 40f; } }

    public Skeleton() : base("Skeleton") { }

    public void GetDamage(float damage)
    {
        hp -= damage;
    }
}
