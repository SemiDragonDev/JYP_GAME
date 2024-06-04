using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{ 
    public Transform itemsParent;  // Parent object to hold the slots
    public GameObject inventoryUI;  // Inventory UI panel
    public GameObject inventorySlotPrefab;  // Prefab for inventory slots
    public int slotCount = 27;  // Number of slots in the inventory
    public bool isInventoryOpen = false;

    Inventory inventory;
    InventorySlot[] slots;

    private SetMouseState mouseState;

    void Start()
    {
        mouseState = FindObjectOfType<SetMouseState>();

        inventory = Inventory.Instance;
        inventory.onInventoryChangedCallback += UpdateUI;

        slots = new InventorySlot[slotCount];

        // Create slots dynamically based on the slotCount
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, itemsParent);
            slots[i] = slotGO.GetComponent<InventorySlot>();
        }

        UpdateUI();  // Initial UI update
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryUI.SetActive(isInventoryOpen);

            if (isInventoryOpen)
            {
                mouseState.UnlockCursor();
            }
            else
            {
                mouseState.LockCursor();
            }
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
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
