using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public virtual float Hp {  get; private set; }
    public virtual float AttackDamage { get; private set; }
    public virtual string Name {  get; private set; }
}
