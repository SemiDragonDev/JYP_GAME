using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryScreen inventoryScreen;
    [SerializeField]
    private UIQuickSlot quickSlot;
    [SerializeField]
    private UIEquipmentScreen equipmentSlot;

    [SerializeField]
    private InventorySO inventory;

    public List<InventorySlot> initialSlots = new List<InventorySlot>();

    private void Start()
    {
        PrepareUI();
        PrepareInventoryData();
    }

    private void PrepareInventoryData()
    {
        inventory.InitContainer();
        inventory.OnInventoryChanged += UpdateInventoryUI;
        foreach (InventorySlot item in initialSlots)
        {
            if (item.IsEmpty)
                continue;
            inventory.AddItem(item);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventorySlot> inventoryState)
    {
        inventoryScreen.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryScreen.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.amount);
        }
    }

    private void PrepareUI()
    {
        inventoryScreen.InitInventoryUI(inventory.InventorySize);
        quickSlot.InitQuickSlotUI(quickSlot.QuickSlotSize);
        equipmentSlot.InitEquipSlotUI(equipmentSlot.EquipmentSlotSize);
        this.inventoryScreen.OnSwapItems += HandleSwapItems;
        this.inventoryScreen.OnStartDragging += HandleDragging;
    }

    private void HandleDragging(int itemIndex)
    {
        InventorySlot inventorySlot = inventory.GetItemAt(itemIndex);
        if (inventorySlot.IsEmpty)
            return;
        inventoryScreen.CreateDraggedItem(inventorySlot.item.ItemImage, inventorySlot.amount);
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex2)
    {
        inventory.SwapItem(itemIndex_1, itemIndex2);
    }

    private void Update()
    {
        inventoryScreen.OpenCloseInventory();
    }
}
