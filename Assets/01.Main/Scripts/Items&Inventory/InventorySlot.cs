using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public Button button;
    public TextMeshProUGUI stackSizeText;

    InventoryItem inventoryItem;

    public void AddItem(InventoryItem newInventoryItem)
    {
        inventoryItem = newInventoryItem;
        if (iconImage != null)
        {
            iconImage.sprite = inventoryItem.item.icon;
            iconImage.color = Color.white;
            iconImage.enabled = true;
        }

        if (stackSizeText != null)
        {
            stackSizeText.text = inventoryItem.item.isStackable ? inventoryItem.stackSize.ToString() : "";
        }
    }

    public void ClearSlot()
    {
        inventoryItem = null;
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
}
