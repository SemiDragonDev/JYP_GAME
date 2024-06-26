using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image iconImage;

    InventoryItem BuildItem { get; set; }

    public void DisplayItem(Item item)
    {
        iconImage.sprite = item.itemImage;
        iconImage.color = Color.white;
    }

    public void ClearSlot()
    {
        iconImage.sprite = null;
        iconImage.color = Color.clear;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!InventorySlot.IsDraggingSlot)  //  DraggingSlot에 아무것도 없을 경우
        {
            // BuildSlot에 있는 아이템을 DraggingSlot으로 옮긴다
        }
        else  //    만일 DraggingSlot에 무언가 들고있다면
        {
           //   DraggingSlot에 있는 아이템은 InventorySlot으로 강제로 옮기고
           //   BuildSlot에 있는 아이템을 DraggingSlot으로 옮긴다
        }
    }
}
