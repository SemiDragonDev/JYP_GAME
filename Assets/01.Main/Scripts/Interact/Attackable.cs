using UnityEngine;

public class Attackable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // Logic for attacking the object
        Debug.Log("Attacked: " + gameObject.name);
        // You can add your attack logic here, such as reducing health, playing animations, etc.
    }
}
