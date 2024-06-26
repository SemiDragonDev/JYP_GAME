using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotInven : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;

    public InventoryItem QuickSlotInvenItem { get; set;}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Inventory.Instance.IsDraggingSlot && this.QuickSlotInvenItem != null)   // 드래깅 중이 아니고, 아이템이 있는 슬롯을 클릭했을 경우
        {
            //  클릭한 아이템을 Dragging Slot으로 옮긴다
        }
        else if (Inventory.Instance.IsDraggingSlot && this.QuickSlotInvenItem == null)   //  드래깅 중이고, 빈 슬롯을 클릭했을 경우
        {
            //  드래깅 중인 아이템을 클릭한 이 슬롯에 옮긴다
        }
        else if (Inventory.Instance.IsDraggingSlot && this.QuickSlotInvenItem != null)  //  드래깅 중이고, 아이템이 있는 슬롯을 클릭했을 경우
        {
            //  드래깅 중인 아이템과 클릭한 슬롯의 아이템을 맞바꾼다
        }
    }
}
