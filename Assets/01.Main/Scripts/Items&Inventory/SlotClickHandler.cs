using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SlotClickHandler : MonoBehaviour, IPointerClickHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private static InventorySlot pickedItemSlot = null;
    private static int pickedItemCount;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (pickedItemSlot != null) // pickedItemSlot�� ���� ���, �� �������� �̹����� Ŀ���� ����ٴϵ��� �����
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, null, out position);

            pickedItemSlot.transform.position = position;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject != null)
        {
            InventorySlot clickedSlot = clickedObject.GetComponent<InventorySlot>();
            if (clickedSlot != null)
            {
                OnSlotClicked(clickedSlot);
            }
        }
    }

    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        if(pickedItemSlot == null)
        {
            PickItem(clickedSlot);
        }
        else
        {
            SwapItems(clickedSlot);
        }
    }

    private void PickItem(InventorySlot slot)
    {
        if (slot.Item == null) return;  // ù Ŭ���� ������ �������� ���� �����̶�� �ƹ��͵� ���� �ʴ´�.

        pickedItemSlot = slot;  // Ŭ���� ������ pickedItemSlot �� �ְ�
        pickedItemCount = slot.stackSizeText.enabled ? int.Parse(slot.stackSizeText.text) : 1;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    private void SwapItems(InventorySlot targetSlot)
    {
        if(pickedItemSlot == targetSlot)    // ù Ŭ���� ������ �ٽ� Ŭ������ ���
        {
            DropItem();
            return;
        }

        var tempItem = targetSlot.Item;
        int tempCount = targetSlot.stackSizeText.enabled ? int.Parse(targetSlot.stackSizeText.text) : 1;

        targetSlot.SetItem(pickedItemSlot.Item.item, pickedItemCount);
        pickedItemSlot.SetItem(tempItem.item, tempCount);

        DropItem();
    }

    private void DropItem()
    {
        pickedItemSlot = null;
        pickedItemCount = 0;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
