using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    public InventorySlot[] slots = new InventorySlot[27];

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, count);
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].inventoryItem.item == item && item.isStackable)
            {
                slots[i].inventoryItem.itemCount += count;
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        slots[slotIndex].ClearSlot();
        OnInventoryChanged?.Invoke();
    }

    public InventorySlot[] GetSlots()
    {
        return slots;
    }
}
