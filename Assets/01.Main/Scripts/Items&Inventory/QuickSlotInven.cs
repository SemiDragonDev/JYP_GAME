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
        if (!Inventory.Instance.IsDraggingSlot && this.QuickSlotInvenItem != null)   // �巡�� ���� �ƴϰ�, �������� �ִ� ������ Ŭ������ ���
        {
            //  Ŭ���� �������� Dragging Slot���� �ű��
        }
        else if (Inventory.Instance.IsDraggingSlot && this.QuickSlotInvenItem == null)   //  �巡�� ���̰�, �� ������ Ŭ������ ���
        {
            //  �巡�� ���� �������� Ŭ���� �� ���Կ� �ű��
        }
        else if (Inventory.Instance.IsDraggingSlot && this.QuickSlotInvenItem != null)  //  �巡�� ���̰�, �������� �ִ� ������ Ŭ������ ���
        {
            //  �巡�� ���� �����۰� Ŭ���� ������ �������� �¹ٲ۴�
        }
    }
}
