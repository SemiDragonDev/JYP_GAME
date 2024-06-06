using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI stackSizeText;

    public InventoryItem Item {  get;  set; }

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

    public void SetItem(Item newItem, int count)
    {
        Item.item = newItem;
        if (newItem != null)
        {
            iconImage.sprite = newItem.icon;
            iconImage.enabled = true;

            if (count > 1)
            {
                stackSizeText.text = count.ToString();
                stackSizeText.enabled = true;
            }
            else
            {
                stackSizeText.enabled = false;
            }
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            stackSizeText.enabled = false;
        }
    }
}
