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

public class Interactor : MonoBehaviour
{
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