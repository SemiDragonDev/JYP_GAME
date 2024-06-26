using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image iconImage;

    public InventoryItem BuildItem { get; set; }

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

    public bool IsEmpty()
    {
        bool empty = BuildItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Inventory.Instance.IsDraggingSlot)  //  DraggingSlot�� �ƹ��͵� ���� ���
        {
            Inventory.Instance.BuildToDragging();
        }
        else  //    ���� DraggingSlot�� ���� ����ִٸ�
        {
           //   DraggingSlot�� �ִ� �������� InventorySlot���� ������ �ű��
           //   BuildSlot�� �ִ� �������� DraggingSlot���� �ű��
        }
    }
}
