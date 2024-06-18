using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    public InventorySlot[] slots; // 이미 에디터에서 설정된 슬롯들

    private void Start()
    {
        // Start 메서드에서 slots 배열의 초기화 상태를 확인
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogError($"Slot {i} is not initialized!");
            }
            else
            {
                Debug.Log($"Slot {i} initialized.");
                slots[i].ClearSlot(); // 모든 슬롯을 명시적으로 초기화
            }
        }
    }

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"Checking slot {i}, IsEmpty: {slots[i].IsEmpty()}"); // 슬롯 상태 확인

            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, count);
                Debug.Log($"Added {item.itemName} to slot {i}"); // 디버그 메시지 추가
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].inventoryItem.item == item && item.isStackable)
            {
                slots[i].inventoryItem.itemCount += count;
                Debug.Log($"Stacked {item.itemName} in slot {i}"); // 디버그 메시지 추가
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning("No empty slots available!"); // 슬롯이 없는 경우 경고 메시지 추가
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
