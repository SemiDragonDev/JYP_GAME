using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem CraftingItem { get; set; }
    Image iconImage;
    TextMeshProUGUI tmpUGUI;

    public int slotIndex;

    private void Start()
    {
        iconImage = GetComponent<Image>();
        tmpUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventorySlot.tempSavingItem != null)
        {
            PlaceItem(InventorySlot.tempSavingItem);
        }

        if(eventData.button == PointerEventData.InputButton.Right)  //  ��Ŭ�� �Է½� ��ٷ� InventorySlot���� ���ư��� �ϱ�
        {

        }
    }

    /// <summary>
    /// tempSavingItem ��, dragging ���� Item�� �Է¹޾�, ���� craftingSlot�� �ϳ��� ��ġ��Ų��
    /// </summary>
    /// <param name="tempSavingItem"></param>
    void PlaceItem(InventoryItem tempSavingItem)
    {
        tempSavingItem.stackSize--;  //  tempSavingItem�� Item �������� stackSize�� �ϳ��� ���ҽ����ش�
        if (this.CraftingItem == null)
        {
            CraftingItem.item = tempSavingItem.item;    //  ��� craftingSlot ���� Item ������ ���� �ϰ�, stackSize�� �ϳ��� ���������ش�
            CraftingItem.stackSize = 1;
        }
        else
        {
            CraftingItem.stackSize++;
            tmpUGUI.text = CraftingItem.stackSize.ToString();
        }

        iconImage.sprite = CraftingItem.item.icon;
        iconImage.color = Color.white;

        if (tempSavingItem.stackSize == 0)
        {
            InventorySlot.draggingImage.sprite = null;
            InventorySlot.draggingImage.color = Color.clear;
            InventorySlot.draggingText.text = "";

            InventorySlot.pickedSlot = null;
        }
    }
}
