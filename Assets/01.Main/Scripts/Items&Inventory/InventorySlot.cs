using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI stackSizeText;
   private Image draggingItemIcon;

    public InventoryItem Item {  get;  set; }

    private static InventorySlot pickedSlot = null;

    public void AddItem(InventoryItem newInventoryItem)
    {
        Item = newInventoryItem;
        if (iconImage != null)
        {
            iconImage.sprite = Item.item.icon;
            iconImage.color = Color.white;
            iconImage.enabled = true;
        }

        if (stackSizeText != null)
        {
            stackSizeText.text = Item.item.isStackable ? Item.stackSize.ToString() : "";
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
        if (pickedSlot == null)
        {
            // Select the current slot and start dragging
            pickedSlot = this;
            StartDragging();
        }
        else
        {
            // Swap items between pickedSlot and current slot
            SwapItems(pickedSlot, this);
            // End dragging
            EndDragging();
            // Deselect the slot
            pickedSlot = null;
        }
    }

    // Function to start dragging the item icon
    private void StartDragging()
    {
        if (draggingItemIcon == null)
        {
            draggingItemIcon = GameObject.FindGameObjectWithTag("DraggingItemIcon").GetComponent<Image>();
        }

        if (Item != null)
        {
            // Use the existing dragging icon
            draggingItemIcon.sprite = pickedSlot.Item.item.icon;
            draggingItemIcon.raycastTarget = false; // Make sure the icon doesn't block raycasts
            draggingItemIcon.gameObject.SetActive(true);
        }
    }

    private void EndDragging()
    {
        if (draggingItemIcon != null)
        {
            draggingItemIcon.gameObject.SetActive(false);
            draggingItemIcon.raycastTarget = true;
            draggingItemIcon = null;
        }
    }

    // Function to swap items between two slots
    private void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        var tempItem = slot1.Item;
        slot1.Item = slot2.Item;
        slot2.Item = tempItem;

        // Update both slots
        slot1.UpdateSlot();
        slot2.UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (Item != null)
        {
            iconImage.sprite = Item.item.icon;
            // Assuming you have an Image component to display the icon in the slot
            GetComponent<Image>().sprite = iconImage.sprite;
            GetComponent<Image>().enabled = true;

            stackSizeText.text = Item.stackSize > 1 ? Item.stackSize.ToString() : "";
            stackSizeText.enabled = true;
        }
        else
        {
            GetComponent<Image>().enabled = false;
            stackSizeText.enabled = false;
        }
    }

    private void Start()
    {
        UpdateSlot();
    }
}
