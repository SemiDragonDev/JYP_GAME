using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    InventoryItem Item { get; set; }
    Image iconImage;

    public int slotIndex;

    private void Start()
    {
        iconImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventorySlot.tempSavingItem != null)
        {
            PlaceItem(InventorySlot.tempSavingItem);
        }
    }

    void PlaceItem(InventoryItem inventoryItem)
    {
        inventoryItem.stackSize--;

        Debug.Log("Picked Slot's Item : " + inventoryItem);
        iconImage.sprite = inventoryItem.item.icon;
        iconImage.color = Color.white;
    }
}
