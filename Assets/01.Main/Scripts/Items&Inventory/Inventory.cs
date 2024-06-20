using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : Singleton<Inventory>
{
    public event Action OnInventoryChanged;

    public List<InventorySlot> slots = new List<InventorySlot>();
    public DraggingSlot draggingSlot;

    private void Start()
    {
        var foundSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();   //  Resources.FindObjectsOfTypeAll�� ��� Active�� �ƴ� ������Ʈ���� ã�ƿ� �� �ִ�. (������ Asset ������ �ִ� ������Ʈ���� ã�ƿ��Ƿ�, prefab�� ������� ��� ������ �ȸ´� ���� ����)
        var sortedSlots = foundSlots.OrderBy(slot => slot.slotIndex).ToList();
        slots = sortedSlots;
        foreach (var slot in slots)
        {
            slot.InventoryItem = null;
        }
        draggingSlot = GameObject.Find("DraggingSlot").GetComponent<DraggingSlot>();
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
    /// Ŭ���� ������ �巡����������� �Ű��ش�
    /// </summary>
    /// <param name="clickedSlot"> Ŭ���� ���� </param>
    public void ToDraggingItem(int slotIndex)
    {
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        Debug.Log("Dragging Slot�� ����ִ� Item : " + draggingSlot.DraggingItem.item.itemName);
        slots[slotIndex].InventoryItem = null;
        OnInventoryChanged?.Invoke();
    }

    public void DraggingItemToEmptySlot(int slotIndex)
    {
        slots[slotIndex].InventoryItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = null;
        OnInventoryChanged?.Invoke();
    }

    public List<InventorySlot> GetSlots()
    {
        return slots;
    }
}
