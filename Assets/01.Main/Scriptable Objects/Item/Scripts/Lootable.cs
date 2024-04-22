using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour, ILootable
{
    public InventorySO inventory;
    private Item item;
    private UIInventoryScreen inventoryScreen;

    public void Loot()
    {
        item = GetComponent<Item>();
        inventory.AddItem(item.item, item.amount);
        Destroy(item.transform.gameObject);
    }
}
