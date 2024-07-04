using UnityEngine;

public class Lootable : MonoBehaviour, IInteractable
{
    public Item item;

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
            Debug.Log($"인벤토리에 {item.itemName} 아이템 추가");
            inventory.AddItem(item, 1); // 아이템을 1개 추가
        }
        else
        {
            Debug.LogError("Inventory not found!");
        }
    }
}
