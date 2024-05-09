using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : FieldEnemy
{
    public override float Hp => base.Hp;
    public override float AttackDamage => base.AttackDamage;
    public override string Name => base.Name;

    public override void SpawnAtNight(int spawnNum, string name)
    {
        base.SpawnAtNight(spawnNum, name);
    }
}
