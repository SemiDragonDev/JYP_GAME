using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;  // Parent object to hold the slots
    public GameObject inventoryUI;  // Inventory UI panel
    public GameObject inventorySlotPrefab;  // Prefab for inventory slots
    public int slotCount = 27;  // Number of slots in the inventory

    Inventory inventory;
    List<InventorySlot> slots = new List<InventorySlot>();

    void Start()
    {
        inventory = Inventory.Instance;
        inventory.onInventoryChangedCallback += UpdateUI;

        // Create slots dynamically based on the slotCount
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, itemsParent);
            InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
            slots.Add(slotComponent);
        }

        UpdateUI();  // Initial UI update
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
