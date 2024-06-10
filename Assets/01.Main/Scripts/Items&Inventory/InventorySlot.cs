using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI stackSizeText;

    private Canvas canvas;
    private CanvasGroup canvasGroup; // 알파값 조정을 위한 캔버스그룹

    public InventoryItem Item { get; set; }

    private static InventorySlot pickedSlot = null;
    private static RectTransform pickedSlotIconRect;
    private static Vector2 originalIconRectInfo;
    private static bool nowDragging = false;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (nowDragging)
        {
            SlotFollowingCursor();
        }
    }

    //  슬롯에 아이템을 할당하고, 아이콘 이미지 등을 보이게 한다
    public void AddItem(InventoryItem newInventoryItem)
    {
        Item = newInventoryItem;
        if (newInventoryItem != null)
        {
            iconImage.sprite = newInventoryItem.item.icon;
            iconImage.color = Color.white;
            iconImage.enabled = true;
            stackSizeText.text = newInventoryItem.stackSize.ToString();
            stackSizeText.enabled = newInventoryItem.stackSize > 1; // 개수가 1보다 클 때만 표시
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        Item = null;
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = Color.clear;
            iconImage.enabled = true;
        }

        if (stackSizeText != null)
        {
            stackSizeText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Slot clicked: {this.name}");

        if (pickedSlot == null) // 처음 클릭하는 슬롯일 경우
        {
            pickedSlot = this; // 클릭한 슬롯을 pickedSlot으로

            if (pickedSlot.Item != null) // 클릭한 슬롯에 아이템이 있다면
            {
                pickedSlotIconRect = pickedSlot.iconImage.GetComponent<RectTransform>(); // pickedSlot의 RectTransform 컴포넌트를 받아옴
                originalIconRectInfo = pickedSlotIconRect.anchoredPosition; // pickedSlot의 위치를 저장해 놓음
                nowDragging = true; // 업데이트에서 조건으로 쓰일 변수
                pickedSlot.iconImage.raycastTarget = false;
                pickedSlot.canvasGroup.blocksRaycasts = false;
                pickedSlot.canvasGroup.alpha = 0.6f;
            }
            else // 클릭한 슬롯에 아이템이 없다면
            {
                pickedSlot = null; // 클릭한 슬롯을 초기화 (아이템이 있는 슬롯을 클릭했을때만 진행되게 한다)
            }
        }
        else // 이미 클릭한 슬롯이 있어서 아이템 아이콘을 드래깅 중일때 두번째 슬롯을 클릭한 경우
        {
            if (this == pickedSlot) // 두 번째 클릭한 슬롯이 처음 클릭한 슬롯인 경우
            {
                // 드래그를 취소하고 아이템을 원래 위치로 되돌림
                ResetDraggingState();
            }
            else if (this.Item != null) // 두 번째 클릭한 슬롯에 아이템이 있는 경우
            {
                SwapItems(pickedSlot, this);
                ResetDraggingState();
            }
            else // 두 번째 클릭한 슬롯에 아이템이 없는 경우
            {
                MoveItem(pickedSlot, this);
                ResetDraggingState();
            }
        }
    }

    private void ResetDraggingState()
    {
        nowDragging = false;

        if (pickedSlot != null)
        {
            pickedSlot.iconImage.raycastTarget = true;
            pickedSlotIconRect.anchoredPosition = originalIconRectInfo;
            pickedSlot.canvasGroup.blocksRaycasts = true;
            pickedSlot.canvasGroup.alpha = 1f;

            pickedSlot = null;
            pickedSlotIconRect = null;
        }
    }

    // 이미 클릭된 아이템 슬롯이 없을때(첫 클릭하는 슬롯일때) 호출된다.
    private void SlotFollowingCursor()
    {
        // 아이템이 든 슬롯이라면, 슬롯의 Rect Transform 을 받아와 커서를 따라가게 만든다.
        Vector2 movePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out movePosition);

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out globalMousePos))
        {
            if (pickedSlotIconRect != null)
            {
                pickedSlotIconRect.position = globalMousePos;
            }
        }
    }

    private void SwapItems(InventorySlot pickedSlot, InventorySlot secondSlot)
    {
        var tempItem = pickedSlot.Item;

        // UI 업데이트
        pickedSlot.AddItem(secondSlot.Item);
        secondSlot.AddItem(tempItem);
    }

    private void MoveItem(InventorySlot pickedSlot, InventorySlot emptySlot)
    {
        // emptySlot이 비어 있는 슬롯일 경우 아이템 이동
        emptySlot.AddItem(pickedSlot.Item);
        pickedSlot.ClearSlot();
    }
}
