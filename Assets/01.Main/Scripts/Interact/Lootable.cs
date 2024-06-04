using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Lootable : MonoBehaviour, IInteractable
{
    public Item item; // The item to be added to the inventory

    public void Interact()
    {
        AddToInventory();
        Destroy(gameObject);
    }

    private void AddToInventory()
    {
        Inventory.Instance.AddItem(item);
    }
}
