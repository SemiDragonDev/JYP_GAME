using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggingSlot : MonoBehaviour
{
    public Image draggingItemImage;
    public TextMeshProUGUI draggingItemCountText;

    private InventoryItem draggingItem;

    void Start()
    {
        draggingItemImage.enabled = false;
        draggingItemCountText.enabled = false;
    }

    public void StartDragging(InventoryItem item)
    {
        draggingItem = item;
        UpdateUI();
        draggingItemImage.enabled = true;
        draggingItemCountText.enabled = item.item.isStackable;
    }

    public void StopDragging()
    {
        draggingItem = null;
        draggingItemImage.enabled = false;
        draggingItemCountText.enabled = false;
    }

    void Update()
    {
        if (draggingItemImage.enabled)
        {
            Vector3 position = Input.mousePosition;
            draggingItemImage.transform.position = position;
            draggingItemCountText.transform.position = position;
        }
    }

    private void UpdateUI()
    {
        draggingItemImage.sprite = draggingItem.item.itemImage;
        draggingItemCountText.text = draggingItem.itemCount.ToString();
    }
}
