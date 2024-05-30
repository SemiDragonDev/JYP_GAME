using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text stackSizeText;

    InventoryItem inventoryItem;

    public void AddItem(InventoryItem newInventoryItem)
    {
        inventoryItem = newInventoryItem;
        icon.sprite = inventoryItem.item.icon;
        icon.enabled = true;
        stackSizeText.text = inventoryItem.item.isStackable ? inventoryItem.stackSize.ToString() : "";
    }

    public void ClearSlot()
    {
        inventoryItem = null;
        icon.sprite = null;
        icon.enabled = false;
        stackSizeText.text = "";
    }

    public void UseItem()
    {
        if (inventoryItem != null)
        {
            Debug.Log("Using " + inventoryItem.item.itemName);
        }
    }
}
