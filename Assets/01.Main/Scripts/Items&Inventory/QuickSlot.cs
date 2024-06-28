using UnityEngine.EventSystems;
using UnityEngine;

public class QuickSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    public InventoryItem QuickSlotItem { get; set; }

    public bool IsEmpty()
    {
        var empty = QuickSlotItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"QuickSlot clicked: slotIndex {this.slotIndex}, IsDraggingSlot: {Inventory.Instance.IsDraggingSlot}, QuickSlotItem: {(Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem != null ? Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem.item.itemName : "null")}");

        if (!Inventory.Instance.IsDraggingSlot && Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem != null)
        {
            Debug.Log("Dragging from QuickSlot");
            Inventory.Instance.QSToDraggingFromGroup1(this.slotIndex);
            Inventory.Instance.QSToDraggingFromGroup2(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem == null)
        {
            Debug.Log("Dragging to empty QuickSlot");
            Inventory.Instance.DraggingToQS(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem != null)
        {
            Debug.Log("Swapping with QuickSlot");
            Inventory.Instance.SwapDraggingAndQS(this.slotIndex);
        }

        // 추가적인 상태 확인 로그
        Debug.Log($"Post-click status: slotIndex {this.slotIndex}, QuickSlotItem: {(Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem != null ? Inventory.quickSlotsGroup1[slotIndex].QuickSlotItem.item.itemName : "null")}, IsDraggingSlot: {Inventory.Instance.IsDraggingSlot}");
    }
}
