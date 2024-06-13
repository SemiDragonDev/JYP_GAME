using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI stackSizeText;

    private Canvas mainCanvas;
    
    private CanvasGroup canvasGroup; // ���İ� ������ ���� ĵ�����׷�

    public InventoryItem Item { get; set; }

    private static InventorySlot pickedSlot = null;
    public static InventoryItem tempSavingItem = null;
    private static Image draggingImage;
    private static TextMeshProUGUI draggingText;

    private static bool nowDragging = false;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
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
                tempSavingItem = pickedSlot.Item;   //  ������ ������ �ӽ÷� �����س��´�
                draggingImage = InventoryManager.Instance.draggingImage;
                draggingText = InventoryManager.Instance.draggingText;
                draggingImage.sprite = pickedSlot.Item.item.icon;
                if (pickedSlot.Item.stackSize > 1)
                {
                    draggingText.text = pickedSlot.Item.stackSize.ToString();
                }  // pickedSlot�� �ִ� �̹����� �ؽ�Ʈ�� dragging ������Ʈ ������ �ű��

                draggingImage.gameObject.SetActive(true);
                draggingImage.color = Color.white;

                this.ClearSlot();
                nowDragging = true; // ������Ʈ���� �������� ���� ����
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
                AddItem(tempSavingItem);
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
            draggingImage.sprite = null;
            draggingImage.color = Color.clear;
            draggingText.text = "";
            draggingImage.gameObject.SetActive(false);

            pickedSlot = null;
            tempSavingItem = null;
        }
    }

    // �̹� Ŭ���� ������ ������ ������(ù Ŭ���ϴ� �����϶�) ȣ��ȴ�.
    private void SlotFollowingCursor()
    {
        // �������� �� �����̶��, ������ Rect Transform �� �޾ƿ� Ŀ���� ���󰡰� �����.
        Vector2 movePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, null, out movePosition);

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, null, out globalMousePos))
        {
            RectTransform rectTransform = InventoryManager.Instance.draggingImageRect;
            if (rectTransform != null)
            {
                rectTransform.position = globalMousePos;
            }
        }
    }

    private void SwapItems(InventorySlot pickedSlot, InventorySlot secondSlot)
    {
        // UI ������Ʈ
        pickedSlot.AddItem(secondSlot.Item);
        secondSlot.AddItem(tempSavingItem);
    }

    private void MoveItem(InventorySlot pickedSlot, InventorySlot emptySlot)
    {
        // emptySlot�� ��� �ִ� ������ ��� ������ �̵�
        emptySlot.AddItem(tempSavingItem);
        pickedSlot.ClearSlot();
    }
}
