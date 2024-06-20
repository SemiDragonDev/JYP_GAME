using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem InventoryItem { get; set; }
    public int slotIndex;

    public bool IsDraggingSlot { get; set; } = false;

    public void AddItem(Item newItem, int count)
    {
        InventoryItem = new InventoryItem(newItem, count);
        Debug.Log($"Added {newItem.itemName} with count {count} to InventorySlot"); // ����� �޽��� �߰�
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
        Debug.Log("Ŭ���� ���� �ε��� : " + this.slotIndex);
        Debug.Log("Ŭ���� ������ InventoryItem�� Null? : " + this.InventoryItem);
        if (!IsDraggingSlot && this.InventoryItem != null)
        {
            Inventory.Instance.ToDraggingItem(this.slotIndex);
            IsDraggingSlot = true;
        }
        else if (IsDraggingSlot && this.InventoryItem == null)
        {
            Inventory.Instance.DraggingItemToEmptySlot(this.slotIndex);
            IsDraggingSlot = false;
        }
    }
}
