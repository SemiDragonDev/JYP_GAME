using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryScreen : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot slotPrefab;
    [SerializeField]
    private RectTransform inventoryPanel;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private InventorySO inventory;
    [SerializeField]
    private ItemOnDrag itemOnDrag;

    List<UIInventorySlot> listOfUISlots = new List<UIInventorySlot>();
    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnStartDragging;
    public event Action<int, int> OnSwapItems;

    private void Awake()
    {
        itemOnDrag.Toggle(false);
    }

    public void InitInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventorySlot uiSlot = Instantiate(slotPrefab);
            uiSlot.transform.SetParent(inventoryPanel);
            listOfUISlots.Add(uiSlot);

            uiSlot.OnItemBeginDrag += HandleBeginDrag;
            uiSlot.OnItemDroppedOn += HandleSwap;
            uiSlot.OnItemEndDrag += HandleEndDrag;
        }
    }

    public void ResetDraggedItem()
    {
        itemOnDrag.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    public void CreateDraggedItem(Sprite sprite, int amount)
    {
        itemOnDrag.Toggle(true);
        itemOnDrag.SetData(sprite, amount);
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemAmount)
    {
        if (listOfUISlots.Count > itemIndex)
        {
            listOfUISlots[itemIndex].SetData(itemImage, itemAmount);
        }
    }

    private void HandleEndDrag(UIInventorySlot slot)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventorySlot slot)
    {
        int index = listOfUISlots.IndexOf(slot);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
    }

    private void HandleBeginDrag(UIInventorySlot slot)
    {
        int index = listOfUISlots.IndexOf(slot);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        OnStartDragging?.Invoke(index);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

    public void OpenCloseInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            if (isActiveAndEnabled == false)
            {
                Show();
                crosshair.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;

                foreach(var item in inventory.GetCurrentInventoryState())
                {
                    UpdateData(item.Key, item.Value.item.ItemImage, item.Value.amount);
                }
            }
            else
            {
                Hide();
                crosshair.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    internal void ResetAllItems()
    {
        foreach (var item in listOfUISlots)
        {
            item.ResetData();
        }
    }
}