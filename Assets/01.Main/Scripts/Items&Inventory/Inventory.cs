using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Start()
    {
        var foundSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();   //  Resources.FindObjectsOfTypeAll을 써야 Active가 아닌 오브젝트까지 찾아올 수 있다. (하지만 Asset 폴더에 있는 오브젝트까지 찾아오므로, prefab을 만들었을 경우 개수가 안맞는 일이 생김)
        var sortedSlots = foundSlots.OrderBy(slot => slot.slotIndex).ToList();
        slots = sortedSlots;
        foreach (var slot in slots)
        {
            slot.InventoryItem = null;
        }
    }

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Debug.Log($"Checking slot {i}, IsEmpty: {slots[i].IsEmpty()}"); // 슬롯 상태 확인

            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, count);
                Debug.Log($"Added {item.itemName} to slot {i}"); // 디버그
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].InventoryItem.item == item && item.isStackable)
            {
                slots[i].InventoryItem.itemCount += count;
                Debug.Log($"Stacked {item.itemName} in slot {i}"); // 디버그
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning("No empty slots available!"); // 슬롯이 없는 경우 경고 메시지 추가
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        slots[slotIndex].ClearSlot();
        OnInventoryChanged?.Invoke();
    }

    public List<InventorySlot> GetSlots()
    {
        return slots;
    }
}
