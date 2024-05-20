using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

interface IGatherable
{
    public void Gather();
}

interface ILootable
{
    public void Loot();
}

interface IAttackable
{
    public void Attack();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private Vector3 hitEffectPos = new Vector3(0f, 0.9f, 0f);
    private static Vector3 checkBoxSize = new Vector3(1f, 1.5f, 1f);
    private static int maxCheckSize = 5;
    public float interactRange = 3f;

    KnockBack knockback;

    public void ClickInteract()
    {
        Collider[] hitColliders = new Collider[maxCheckSize];
        int numOfCollider = Physics.OverlapBoxNonAlloc(this.transform.position, checkBoxSize, hitColliders, Quaternion.identity, layerMask);

        for (int i = 0; i < numOfCollider; i++)
        {
            // Interact 상대가 Enemy 인 경우
            if (hitColliders[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.GetDamage(50f);
                Debug.Log(enemy.Hp);
                var vfx = ObjectPool.Instance.GetPooledObject("HitEffect");
                vfx.transform.position = hitColliders[i].transform.position + hitEffectPos;
                knockback = hitColliders[i].GetComponentInChildren<KnockBack>();
                knockback.PlayingKnockBack();
                StartCoroutine(vfx.CoroutineRelease(0.5f));
            }
            //  Interact 상대가 Gatherable 자원인 경우
            if (hitColliders[i].TryGetComponent<Gatherable>(out Gatherable gatherable))
            {
                gatherable.Gather();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
                else if(hitInfo.collider.gameObject.TryGetComponent(out ILootable lootableObj))
                {
                    lootableObj.Loot();
                }
            }
        }
    }
}