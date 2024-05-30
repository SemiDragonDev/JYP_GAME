using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactionRange = 2f;
    public LayerMask interactableLayer;
    private IInteractable currentInteractable;
    private BoxCollider interactorCollider;

    void Start()
    {
        interactorCollider = GetComponent<BoxCollider>();
        if (interactorCollider != null)
        {
            // Adjust the size and position of the BoxCollider
            interactorCollider.size = new Vector3(1, 1, interactionRange);
            interactorCollider.center = new Vector3(0, 0, interactionRange / 2);
        }
        else
        {
            Debug.LogError("BoxCollider component not found on Interactor GameObject");
        }
    }

    void Update()
    {
        DetectInteractable();

        if (currentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentInteractable.GetInteractType() == "Lootable" || currentInteractable.GetInteractType() == "Interactable")
                {
                    currentInteractable.Interact();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (currentInteractable.GetInteractType() == "Gatherable" || currentInteractable.GetInteractType() == "Attackable")
                {
                    currentInteractable.Interact();
                }
            }
        }
    }

    void DetectInteractable()
    {
        Collider[] hits = Physics.OverlapBox(transform.position + transform.forward * interactionRange / 2, interactorCollider.size / 2, transform.rotation, interactableLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    Debug.Log("Detected interactable: " + currentInteractable.GetInteractType());
                    return;
                }
            }
        }

        currentInteractable = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Entered interactable: " + currentInteractable.GetInteractType());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Exited interactable: " + interactable.GetInteractType());
        }
    }
}
