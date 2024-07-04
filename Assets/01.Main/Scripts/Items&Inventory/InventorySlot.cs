using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem InventoryItem { get; set; }
    public int slotIndex;

    public void AddItem(Item newItem, int count)
    {
        InventoryItem = new InventoryItem(newItem, count);
        Debug.Log($"인벤토리 슬롯에  {newItem.itemName} 아이템 {count} 개 추가"); // 디버그 메시지 추가
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
        Debug.Log($"IsDraggingSlot: {Inventory.Instance.IsDraggingSlot}");

        if (!Inventory.Instance.IsDraggingSlot && this.InventoryItem != null)
        {
            Inventory.Instance.ToDraggingItem(this.slotIndex);
            Inventory.Instance.IsDraggingSlot = true;
            Debug.Log("드래깅 시작한 슬롯 :  " + this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && IsEmpty())
        {
            Inventory.Instance.DraggingItemToEmptySlot(this.slotIndex);
            Inventory.Instance.IsDraggingSlot = false;
            Debug.Log("드래깅 아이템을 드롭한 슬롯 :  " + this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && !IsEmpty())
        {
            Inventory.Instance.SwapWithDraggingItem(this.slotIndex);
        }
    }
}
