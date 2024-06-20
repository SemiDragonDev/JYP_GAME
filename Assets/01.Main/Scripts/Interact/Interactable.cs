using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // Logic for interacting with the object
        Debug.Log("Interacted with: " + gameObject.name);
        // You can add your interaction logic here, such as triggering events, opening doors, etc.
    }
}
