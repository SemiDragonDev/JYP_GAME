using UnityEngine;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public InventoryItem inventoryItem;
    private DraggingSlot draggingSlot;

    void Start()
    {
        draggingSlot = FindObjectOfType<DraggingSlot>();
        ClearSlot();
    }

    public void AddItem(Item newItem, int count)
    {
        inventoryItem = new InventoryItem(newItem, count);
    }

    public void ClearSlot()
    {
        inventoryItem = null;
    }

    public bool IsEmpty()
    {
        return inventoryItem == null;
    }

    public void OnSlotClicked()
    {
        if (!IsEmpty())
        {
            draggingSlot.StartDragging(inventoryItem);
            ClearSlot();
        }
    }
}
