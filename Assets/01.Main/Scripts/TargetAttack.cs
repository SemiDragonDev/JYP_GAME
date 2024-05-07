using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttack : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private Vector3 effectPos = new Vector3(0f, 0.9f, 0f);
    private Vector3 boxSize = new Vector3(1f, 1.5f, 1f);

    KnockBack knockback;

    public void ClickAttack()
    {
        int maxCollider = 5;
        Collider[] hitColliders = new Collider[maxCollider];
        int numColliders = Physics.OverlapBoxNonAlloc(transform.position, boxSize, hitColliders, Quaternion.identity, layerMask);
        for (int i = 0; i < numColliders; i++)
        {
            Debug.Log(hitColliders[i].name);
            var vfx = ObjectPool.Instance.GetPooledObject("HitEffect");
            vfx.transform.position = hitColliders[i].transform.position + effectPos;
            knockback = hitColliders[i].GetComponentInChildren<KnockBack>();
            knockback.PlayingKnockBack();
            StartCoroutine(vfx.CoroutineRelease(0.5f));
        }
    }
}
