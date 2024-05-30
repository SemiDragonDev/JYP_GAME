using UnityEngine;

public class Gatherable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // Logic for gathering the item
        Debug.Log("Gathered item: " + gameObject.name);
        // You can add your gathering logic here, such as adding resources to inventory, playing animations, etc.
        Destroy(gameObject);
    }

    public string GetInteractType()
    {
        return "Gatherable";
    }
}
