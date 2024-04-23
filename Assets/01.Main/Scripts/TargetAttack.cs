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
    ObjectPoolManager poolManager;

    private void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        player = GetComponentInParent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isTargetInCollider = true;
            target = other.gameObject;
            knockback = target.GetComponent<KnockBack>();
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
            Debug.Log("�˹����");
            knockback.PlayingKnockBack();
        }
    }
}
