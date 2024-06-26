using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem InventoryItem { get; set; }

    public int slotIndex;

    public CraftingSystem craftingSystem;

    public bool IsEmpty()
    {
        bool empty = InventoryItem == null;
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventorySlot.IsDraggingSlot)
        {
            Inventory.Instance.DraggingToCrafting(this.slotIndex);

            craftingSystem.OnCraftingChanged();
        }
    }
}
