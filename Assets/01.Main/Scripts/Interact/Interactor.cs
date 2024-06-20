using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactionRange = 3f;
    public Vector3 interactionBoxSize = new Vector3(2f, 2f, 2f);
    public LayerMask interactableLayer;

    private Collider[] interactables = new Collider[10];

    private void Update()
    {
        InteractWithEKey();
    }

    public void Interact()
    {
        Vector3 interactionCenter = transform.position + transform.forward * interactionRange / 2 + transform.up * interactionRange / 2;
        int numInteractables = Physics.OverlapBoxNonAlloc(interactionCenter, interactionBoxSize / 2, interactables, Quaternion.identity, interactableLayer);

        //  오브젝트 하나가 여러번 입력되는 문제가 발생해서, 이미 Interact가 끝난 Object는 interactedObjects에 넣어놓는 방법
        HashSet<IInteractable> interactedObjects = new HashSet<IInteractable>();

        for (int i = 0; i < numInteractables; i++)
        {
            IInteractable interactable = interactables[i].GetComponent<IInteractable>();
            if (interactable != null && !interactedObjects.Contains(interactable))
            {
                interactable.Interact();
                interactedObjects.Add(interactable);
            }
        }
    }

    public void InteractWithEKey()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 interactionCenter = transform.position + transform.forward * interactionRange / 2 + transform.up * interactionRange / 2;
            int numInteractables = Physics.OverlapBoxNonAlloc(interactionCenter, interactionBoxSize / 2, interactables, Quaternion.identity, interactableLayer);

            for (int i = 0; i < numInteractables; i++)
            {
                IInteractable interactable = interactables[i].GetComponent<IInteractable>();
                if (interactable != null && interactable is Lootable lootable)
                {
                    lootable.Interact();
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 interactionCenter = transform.position + transform.forward * interactionRange / 2 + transform.up * interactionRange / 2;
        Gizmos.DrawWireCube(interactionCenter, interactionBoxSize);
    }
}
