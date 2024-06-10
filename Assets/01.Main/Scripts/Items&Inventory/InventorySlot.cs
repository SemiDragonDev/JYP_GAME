using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI stackSizeText;

    private Canvas canvas;
    private CanvasGroup canvasGroup; // ���İ� ������ ���� ĵ�����׷�

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

    //  ���Կ� �������� �Ҵ��ϰ�, ������ �̹��� ���� ���̰� �Ѵ�
    public void AddItem(InventoryItem newInventoryItem)
    {
        Item = newInventoryItem;
        if (newInventoryItem != null)
        {
            iconImage.sprite = newInventoryItem.item.icon;
            iconImage.color = Color.white;
            iconImage.enabled = true;
            stackSizeText.text = newInventoryItem.stackSize.ToString();
            stackSizeText.enabled = newInventoryItem.stackSize > 1; // ������ 1���� Ŭ ���� ǥ��
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

        if (pickedSlot == null) // ó�� Ŭ���ϴ� ������ ���
        {
            pickedSlot = this; // Ŭ���� ������ pickedSlot����

            if (pickedSlot.Item != null) // Ŭ���� ���Կ� �������� �ִٸ�
            {
                pickedSlotIconRect = pickedSlot.iconImage.GetComponent<RectTransform>(); // pickedSlot�� RectTransform ������Ʈ�� �޾ƿ�
                originalIconRectInfo = pickedSlotIconRect.anchoredPosition; // pickedSlot�� ��ġ�� ������ ����
                nowDragging = true; // ������Ʈ���� �������� ���� ����
                pickedSlot.iconImage.raycastTarget = false;
                pickedSlot.canvasGroup.blocksRaycasts = false;
                pickedSlot.canvasGroup.alpha = 0.6f;
            }
            else // Ŭ���� ���Կ� �������� ���ٸ�
            {
                pickedSlot = null; // Ŭ���� ������ �ʱ�ȭ (�������� �ִ� ������ Ŭ���������� ����ǰ� �Ѵ�)
            }
        }
        else // �̹� Ŭ���� ������ �־ ������ �������� �巡�� ���϶� �ι�° ������ Ŭ���� ���
        {
            if (this == pickedSlot) // �� ��° Ŭ���� ������ ó�� Ŭ���� ������ ���
            {
                // �巡�׸� ����ϰ� �������� ���� ��ġ�� �ǵ���
                ResetDraggingState();
            }
            else if (this.Item != null) // �� ��° Ŭ���� ���Կ� �������� �ִ� ���
            {
                SwapItems(pickedSlot, this);
                ResetDraggingState();
            }
            else // �� ��° Ŭ���� ���Կ� �������� ���� ���
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

    // �̹� Ŭ���� ������ ������ ������(ù Ŭ���ϴ� �����϶�) ȣ��ȴ�.
    private void SlotFollowingCursor()
    {
        // �������� �� �����̶��, ������ Rect Transform �� �޾ƿ� Ŀ���� ���󰡰� �����.
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

        // UI ������Ʈ
        pickedSlot.AddItem(secondSlot.Item);
        secondSlot.AddItem(tempItem);
    }

    private void MoveItem(InventorySlot pickedSlot, InventorySlot emptySlot)
    {
        // emptySlot�� ��� �ִ� ������ ��� ������ �̵�
        emptySlot.AddItem(pickedSlot.Item);
        pickedSlot.ClearSlot();
    }
}
