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
            Debug.Log("ƒ¸ΩΩ∑‘ø°º≠ µÂ∑°±Î Ω√¿€");
            Inventory.Instance.IQSToDragging(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.invenQuickSlots[slotIndex].InvenQuickSlotItem == null)
        {
            Debug.Log("ƒ¸ΩΩ∑‘¿∏∑Œ æ∆¿Ã≈€ µÂ∑°±Î");
            Inventory.Instance.DraggingToIQS(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.invenQuickSlots[slotIndex].InvenQuickSlotItem != null)
        {
            Debug.Log("ƒ¸ΩΩ∑‘∞˙ æ∆¿Ã≈€ Ω∫ø“");
            Inventory.Instance.SwapDraggingAndIQS(this.slotIndex);
        }
    }
}
