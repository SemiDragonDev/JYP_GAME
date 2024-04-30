using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private bool isTargetInCollider;

    private Transform player;

    KnockBack knockback;

    private void Start()
    {
        player = GetComponentInParent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isTargetInCollider = true;
            target = other.gameObject;
            knockback = target.GetComponentInChildren<KnockBack>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isTargetInCollider = false;
            target = null;
        }
    }

    public void ClickAttack()
    {
        if(isTargetInCollider)
        {
            knockback.PlayingKnockBack();
        }
    }
}
