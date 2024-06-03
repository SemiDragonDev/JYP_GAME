using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxInventorySize = 27;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public bool AddItem(Item item, int amount = 1)
    {
        if (!item.isStackable)
        {
            if (items.Count >= maxInventorySize)
            {
                Debug.Log("Not enough space in inventory");
                return false;
            }

            items.Add(new InventoryItem(item, amount));
            onInventoryChangedCallback?.Invoke();
            return true;
        }
        else
        {
            foreach (InventoryItem inventoryItem in items)
            {
                if (inventoryItem.item == item && inventoryItem.stackSize < item.maxStackSize)
                {
                    int availableSpace = item.maxStackSize - inventoryItem.stackSize;
                    if (amount <= availableSpace)
                    {
                        inventoryItem.stackSize += amount;
                        onInventoryChangedCallback?.Invoke();
                        return true;
                    }
                    else
                    {
                        inventoryItem.stackSize = item.maxStackSize;
                        amount -= availableSpace;
                    }
                }
            }

            while (amount > 0)
            {
                int stackAmount = Mathf.Min(amount, item.maxStackSize);
                if (items.Count >= maxInventorySize)
                {
                    Debug.Log("Not enough space in inventory");
                    return false;
                }
                items.Add(new InventoryItem(item, stackAmount));
                amount -= stackAmount;
            }

            onInventoryChangedCallback?.Invoke();
            return true;
        }
    }
}
