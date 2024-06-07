using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI stackSizeText;

    public InventoryItem Item {  get;  set; }

    private static InventorySlot pickedSlot = null;
    private static Image draggingItemIcon;

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
        if (Item != null)
        {
            // Use the existing dragging icon
            draggingItemIcon.sprite = iconImage.sprite;
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

        // Find the dragging icon in the cursor object
        if (draggingItemIcon == null)
        {
            draggingItemIcon = GameObject.Find("DraggingItemIcon").GetComponent<Image>();
            draggingItemIcon.gameObject.SetActive(false); // Initially inactive
        }
    }

    void Update()
    {
        // Update the dragging icon position every frame
        if (draggingItemIcon != null && draggingItemIcon.gameObject.activeSelf)
        {
            // Position is handled by the CursorFollower script
        }
    }

}
