using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    public InventoryItem QuickSlotItem { get; set; }

    public Image highlightImage; // 슬롯 강조 표시용 이미지

    public bool IsEmpty()
    {
        var empty = QuickSlotItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Inventory.Instance.IsDraggingSlot && Inventory.quickSlots[slotIndex].QuickSlotItem != null)
        {
            Debug.Log("퀵슬롯에서 드래깅 시작");
            Inventory.Instance.QSToDragging(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.quickSlots[slotIndex].QuickSlotItem == null)
        {
            Debug.Log("퀵슬롯으로 아이템 드래깅");
            Inventory.Instance.DraggingToQS(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.quickSlots[slotIndex].QuickSlotItem != null)
        {
            Debug.Log("퀵슬롯과 아이템 스왑");
            Inventory.Instance.SwapDraggingAndQS(this.slotIndex);
        }
     }

    public void Highlight(bool isHighlighted)
    {
        highlightImage.enabled = isHighlighted; // 강조 표시 이미지 활성화/비활성화
    }
}
