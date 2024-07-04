using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    public InventoryItem QuickSlotItem { get; set; }

    public Image highlightImage; // ���� ���� ǥ�ÿ� �̹���

    public bool IsEmpty()
    {
        var empty = QuickSlotItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Inventory.Instance.IsDraggingSlot && Inventory.quickSlots[slotIndex].QuickSlotItem != null)
        {
            Debug.Log("�����Կ��� �巡�� ����");
            Inventory.Instance.QSToDragging(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.quickSlots[slotIndex].QuickSlotItem == null)
        {
            Debug.Log("���������� ������ �巡��");
            Inventory.Instance.DraggingToQS(this.slotIndex);
        }
        else if (Inventory.Instance.IsDraggingSlot && Inventory.quickSlots[slotIndex].QuickSlotItem != null)
        {
            Debug.Log("�����԰� ������ ����");
            Inventory.Instance.SwapDraggingAndQS(this.slotIndex);
        }
     }

    public void Highlight(bool isHighlighted)
    {
        highlightImage.enabled = isHighlighted; // ���� ǥ�� �̹��� Ȱ��ȭ/��Ȱ��ȭ
    }
}
