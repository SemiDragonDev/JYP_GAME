using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public virtual float Hp {  get; }
    public virtual float AttackDamage { get;  }
}
