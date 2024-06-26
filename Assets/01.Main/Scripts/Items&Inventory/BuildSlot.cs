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
        if (!InventorySlot.IsDraggingSlot)  //  DraggingSlot�� �ƹ��͵� ���� ���
        {
            // BuildSlot�� �ִ� �������� DraggingSlot���� �ű��
        }
        else  //    ���� DraggingSlot�� ���� ����ִٸ�
        {
           //   DraggingSlot�� �ִ� �������� InventorySlot���� ������ �ű��
           //   BuildSlot�� �ִ� �������� DraggingSlot���� �ű��
        }
    }
}
