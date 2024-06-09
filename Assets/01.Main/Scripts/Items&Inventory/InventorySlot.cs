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

    public InventoryItem Item {  get;  set; }

    private static InventorySlot pickedSlot = null;
    private static RectTransform pickedSlotRect;
    private static Vector2 originalRectInfo;
    private static bool nowDragging = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
            iconImage.enabled = false;
        }

        if (stackSizeText != null)
        {
            stackSizeText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (pickedSlot == null) //  ó�� Ŭ���ϴ� ������ ���
        {
            pickedSlot = this;  //  Ŭ���� ������ pickedSlot ����

            if (pickedSlot.Item != null)    //  Ŭ���� ���Կ� �������� �ִٸ�
            {
                pickedSlotRect = pickedSlot.gameObject.GetComponent<RectTransform>();   //  pickedSlot�� RectTransform ������Ʈ�� �޾ƿ�
                originalRectInfo = pickedSlotRect.anchoredPosition;    //  pickedSlot�� ��ġ�� ������ ����
                nowDragging = true; //  ������Ʈ���� �������� ���� variable
                pickedSlot.iconImage.raycastTarget = false;
            }
            else    //  Ŭ���� ���Կ� �������� ���ٸ�
            {
                pickedSlot = null;  //  Ŭ���� ������ �ʱ�ȭ (�������� �ִ� ������ Ŭ���������� ����ǰ� �Ѵ�)
            }
        }
        else    //  �̹� Ŭ���� ������ �־ ������ �������� �巡�� ���϶� �ι�° ������ Ŭ���� ���
        {
            // Swap items between pickedSlot and current slot
            SwapItems(pickedSlot, this);

            nowDragging = false;

            pickedSlot.iconImage.raycastTarget = true;
            pickedSlotRect.anchoredPosition = originalRectInfo;

            pickedSlot = null;
            Debug.Log(pickedSlot);
            pickedSlotRect = null;
            Debug.Log(pickedSlotRect);
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
            if (pickedSlotRect != null)
            {
                pickedSlotRect.position = globalMousePos;
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
}
