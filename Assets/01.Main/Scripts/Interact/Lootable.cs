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
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(item, 1); // �������� 1�� �߰�
        }
        else
        {
            Debug.LogError("Inventory not found!");
        }
    }
}
