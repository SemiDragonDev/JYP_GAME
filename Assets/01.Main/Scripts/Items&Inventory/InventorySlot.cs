using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Sprite icon;
    public Button button;
    public TextMeshProUGUI stackSizeText;

    InventoryItem inventoryItem;

    public void AddItem(InventoryItem newInventoryItem)
    {
        inventoryItem = newInventoryItem;
        icon = inventoryItem.item.icon;
        stackSizeText.text = inventoryItem.item.isStackable ? inventoryItem.stackSize.ToString() : "";
    }

    public void ClearSlot()
    {
        inventoryItem = null;
        icon = null;
        stackSizeText.text = "";
    }
}
