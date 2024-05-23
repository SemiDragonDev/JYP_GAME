using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour, IDamagable
{
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
        
    }
}
