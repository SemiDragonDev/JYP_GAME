using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public Button button;
    public TextMeshProUGUI stackSizeText;

    public InventoryItem Item {  get; private set; }

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
}
