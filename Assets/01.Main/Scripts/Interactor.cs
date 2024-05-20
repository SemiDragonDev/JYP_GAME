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

    private static Vector3 checkBoxSize = new Vector3(1f, 1.5f, 1f);
    private static int maxCheckSize = 5;
    public float interactRange = 3f;

    public void ClickInteract()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IGatherable gatherObj))
            {
                gatherObj.Gather();
            }
            //else if(hitInfo.collider.gameObject.TryGetComponent(out IAttackable attackableObj))
            //{
            //    attackableObj.Attack();
            //}
        }
    }

    public void ClickInteract2()
    {
        Collider[] hitColliders = new Collider[maxCheckSize];
        int numOfCollider = Physics.OverlapBoxNonAlloc(this.transform.position, checkBoxSize, hitColliders, Quaternion.identity, layerMask);

        for (int i = 0; i < numOfCollider; i++)
        {
            // Interact 상대가 Enemy 인 경우
            if (hitColliders[i].GetComponent<Enemy>())
            {
                
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