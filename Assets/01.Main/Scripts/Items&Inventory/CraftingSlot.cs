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

        if(eventData.button == PointerEventData.InputButton.Right)  //  우클릭 입력시 곧바로 InventorySlot으로 돌아가게 하기
        {

        }
    }

    /// <summary>
    /// tempSavingItem 즉, dragging 중인 Item을 입력받아, 현재 craftingSlot에 하나씩 위치시킨다
    /// </summary>
    /// <param name="tempSavingItem"></param>
    void PlaceItem(InventoryItem tempSavingItem)
    {
        tempSavingItem.stackSize--;  //  tempSavingItem의 Item 정보에서 stackSize를 하나씩 감소시켜준다
        if (this.CraftingItem == null)
        {
            CraftingItem.item = tempSavingItem.item;    //  대신 craftingSlot 또한 Item 정보를 갖게 하고, stackSize를 하나씩 증가시켜준다
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
