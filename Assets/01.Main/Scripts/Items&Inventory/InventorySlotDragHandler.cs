using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        GameObject target = eventData.pointerEnter;

        if (target != null && target.GetComponent<InventorySlot>() != null)
        {
            // Swap items between slots
            InventorySlot targetSlot = target.GetComponent<InventorySlot>();
            InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();

            InventoryItem tempItem = originalSlot.Item;
            originalSlot.AddItem(targetSlot.Item);
            targetSlot.AddItem(tempItem);
        }
        else
        {
            // Drop item to the world space
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();
            worldPosition.z = 0;
            Instantiate(originalSlot.Item.item.itemPrefab, worldPosition, Quaternion.identity);
            originalSlot.ClearSlot();
        }

        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
