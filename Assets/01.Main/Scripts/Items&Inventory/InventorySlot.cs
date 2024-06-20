using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem InventoryItem { get; set; }
    public int slotIndex;

    public bool NowDraggingSlot { get; set; }

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
        Debug.Log($"IsEmpty called, result: {empty}"); // 디버그 메시지 추가
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!NowDraggingSlot)
        {
            Inventory.Instance.ToDraggingItem(slotIndex);
            NowDraggingSlot = true;
        }
        
    }
}
