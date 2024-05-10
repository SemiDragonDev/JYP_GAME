using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : FieldEnemy
{
    public override float Hp { get { return 100f; } }
    public override float AttackDamage { get { return 40f; } }

    public Skeleton() : base("Skeleton") { }
}
