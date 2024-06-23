using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem InventoryItem { get; set; }
    public int slotIndex;

    public static bool IsDraggingSlot { get; set; } = false;

    public void AddItem(Item newItem, int count)
    {
        InventoryItem = new InventoryItem(newItem, count);
        Debug.Log($"Added {newItem.itemName} with count {count} to InventorySlot"); // 디버그 메시지 추가
    }

    public void ClearSlot()
    {
        InventoryItem = null;
    }

    public bool IsEmpty()
    {
        bool empty = InventoryItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("클릭한 슬롯 인덱스 : " + this.slotIndex);
        Debug.Log("클릭한 슬롯의 InventoryItem이 Null? : " + (this.InventoryItem == null));
        Debug.Log($"IsDraggingSlot: {IsDraggingSlot}");

        if (!IsDraggingSlot && this.InventoryItem != null)
        {
            Inventory.Instance.ToDraggingItem(this.slotIndex);
            IsDraggingSlot = true;
            Debug.Log("Started dragging item from slot " + this.slotIndex);
        }
        else if (IsDraggingSlot && IsEmpty())
        {
            Inventory.Instance.DraggingItemToEmptySlot(this.slotIndex);
            IsDraggingSlot = false;
            Debug.Log("Dropped dragging item to empty slot " + this.slotIndex);
        }
        else if (IsDraggingSlot && !IsEmpty())
        {
            Inventory.Instance.SwapWithDraggingItem(this.slotIndex);
        }
    }
}
