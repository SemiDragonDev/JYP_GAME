using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenQuickSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;

    public InventoryItem InvenQuickSlotItem { get; set; }

    public bool IsEmpty()
    {
        var empty = InvenQuickSlotItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Inventory.Instance.IsDraggingSlot && Inventory.invenQuickSlots[slotIndex].InvenQuickSlotItem != null)
        {
            Debug.Log("�����Կ��� �巡�� ����");
            Inventory.Instance.IQSToDragging(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.invenQuickSlots[slotIndex].InvenQuickSlotItem == null)
        {
            Debug.Log("���������� ������ �巡��");
            Inventory.Instance.DraggingToIQS(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.invenQuickSlots[slotIndex].InvenQuickSlotItem != null)
        {
            Debug.Log("�����԰� ������ ����");
            Inventory.Instance.SwapDraggingAndIQS(this.slotIndex);
        }
    }
}
