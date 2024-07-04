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
        Debug.Log($"�κ��丮 ���Կ�  {newItem.itemName} ������ {count} �� �߰�"); // ����� �޽��� �߰�
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
        Debug.Log("Ŭ���� ������ InventoryItem�� Null? : " + (this.InventoryItem == null));
        Debug.Log($"IsDraggingSlot: {Inventory.Instance.IsDraggingSlot}");

        if (!Inventory.Instance.IsDraggingSlot && this.InventoryItem != null)
        {
            Inventory.Instance.ToDraggingItem(this.slotIndex);
            Inventory.Instance.IsDraggingSlot = true;
            Debug.Log("�巡�� ������ ���� :  " + this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && IsEmpty())
        {
            Inventory.Instance.DraggingItemToEmptySlot(this.slotIndex);
            Inventory.Instance.IsDraggingSlot = false;
            Debug.Log("�巡�� �������� ����� ���� :  " + this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && !IsEmpty())
        {
            Inventory.Instance.SwapWithDraggingItem(this.slotIndex);
        }
    }
}
