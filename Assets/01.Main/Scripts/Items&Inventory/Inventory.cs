using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : Singleton<Inventory>
{
    public event Action OnInventoryChanged;

    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<CraftingSlot> craftingSlots = new List<CraftingSlot>();
    public BuildSlot buildSlot;
    public List<QuickSlotInven> quickSlotInvens = new List<QuickSlotInven>();
    public DraggingSlot draggingSlot;

    public bool IsDraggingSlot { get; set; } = false;

    private void Start()
    {
        var foundSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();   //  Resources.FindObjectsOfTypeAll�� ��� Active�� �ƴ� ������Ʈ���� ã�ƿ� �� �ִ�. (������ Asset ������ �ִ� ������Ʈ���� ã�ƿ��Ƿ�, prefab�� ������� ��� ������ �ȸ´� ���� ����)
        var sortedSlots = foundSlots.OrderBy(slot => slot.slotIndex).ToList();
        slots = sortedSlots;
        foreach (var slot in slots)
        {
            slot.InventoryItem = null;
        }
        var foundCraftingSlots = Resources.FindObjectsOfTypeAll<CraftingSlot>();
        var sortedCraftingSlots = foundCraftingSlots.OrderBy(slot => slot.slotIndex).ToList();
        craftingSlots = sortedCraftingSlots;
        foreach(var craftingSlot in craftingSlots)
        {
            craftingSlot.InventoryItem = null;
        }
        
        draggingSlot = GameObject.Find("DraggingSlot").GetComponent<DraggingSlot>();

        var foundQuickSlotInvens = Resources.FindObjectsOfTypeAll<QuickSlotInven>();
        var sortedQuickSlotInvens = foundQuickSlotInvens.OrderBy(slots => slots.slotIndex).ToList();
        quickSlotInvens = sortedQuickSlotInvens;
        foreach(var quickSlot in quickSlotInvens)
        {
            quickSlot.QuickSlotInvenItem = null;
        }
    }

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, count);
                Debug.Log($"Added {item.itemName} to slot {i}"); // �����
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].InventoryItem.item == item && item.isStackable)
            {
                slots[i].InventoryItem.itemCount += count;
                Debug.Log($"Stacked {item.itemName} in slot {i}"); // �����
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning("No empty slots available!"); // ������ ���� ��� ��� �޽��� �߰�
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        slots[slotIndex].ClearSlot();
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Ŭ���� �κ��丮 ������ �巡����������� �Ű��ش�
    /// </summary>
    /// <param name="clickedSlot"> Ŭ���� ���� </param>
    public void ToDraggingItem(int slotIndex)
    {
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        Debug.Log("Dragging Slot�� ����ִ� Item : " + draggingSlot.DraggingItem.item.itemName);
        slots[slotIndex].InventoryItem = null;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Ŭ���� BuildSlot�� dragging ���������� �ű��.
    /// </summary>
    public void BuildToDragging()
    {
        if (draggingSlot.DraggingItem == null)
        {
            draggingSlot.DraggingItem = buildSlot.BuildItem;
            Debug.Log("Dragging Slot�� ����ִ� Item : " + draggingSlot.DraggingItem.item.itemName);
            IsDraggingSlot = true;
            buildSlot.BuildItem = null;

            foreach(var slot in craftingSlots)
            {
                slot.InventoryItem = null;
            }

            OnInventoryChanged?.Invoke();
        }
    }

    /// <summary>
    /// Dragging ���� �������� �� �κ��丮 ���Կ� ���´�
    /// </summary>
    /// <param name="slotIndex"> Ŭ���� �κ��丮 ������ �ε��� </param>
    public void DraggingItemToEmptySlot(int slotIndex)
    {
        slots[slotIndex].InventoryItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = null;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Dragging ���� �����۰� �κ��丮 ���Կ� �ִ� �������� �¹ٲ۴�
    /// </summary>
    /// <param name="slotIndex"> Ŭ���� �κ��丮 ������ �ε��� </param>
    public void SwapWithDraggingItem(int slotIndex)
    {
        var tempItemSave = new InventoryItem(null, 0);
        tempItemSave = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        slots[slotIndex].InventoryItem = tempItemSave;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Dragging ���� �������� Crafing ���Կ� ���´�
    /// </summary>
    /// <param name="slotIndex"> Ŭ���� Crafting ������ �ε��� </param>
    public void DraggingToCrafting(int slotIndex)
    {
        if (draggingSlot.DraggingItem == null)
        {
            Debug.LogWarning("No item is being dragged.");
            return;
        }

        if (craftingSlots[slotIndex].InventoryItem == null)
        {
            craftingSlots[slotIndex].InventoryItem = new InventoryItem(draggingSlot.DraggingItem.item, 1);
        }
        else if (craftingSlots[slotIndex].InventoryItem.item == draggingSlot.DraggingItem.item && draggingSlot.DraggingItem.item.isStackable)
        {
            craftingSlots[slotIndex].InventoryItem.itemCount++;
        }
        else
        {
            Debug.LogWarning("Crafting slot already contains a different item.");
            return;
        }

        draggingSlot.DraggingItem.itemCount--;

        if (draggingSlot.DraggingItem.itemCount <= 0)
        {
            draggingSlot.DraggingItem = null;
            Inventory.Instance.IsDraggingSlot = false;
        }

        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// QuickSlotInven �� �ִ� ������ Ŭ���� �巡�뽽������ �ű��
    /// </summary>
    /// <param name="slotIndex"></param>
    public void QSIToDragging(int slotIndex)
    {

    }


    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

    public List<CraftingSlot> GetCraftingSlots()
    {
        return craftingSlots;
    }
}
