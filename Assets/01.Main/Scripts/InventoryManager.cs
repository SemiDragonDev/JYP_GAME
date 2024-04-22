using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Image itemImage;


    [SerializeField]
    private UIInventorySlot slotPrefab;
    [SerializeField]
    private RectTransform equipmentPanel;
    [SerializeField]
    private RectTransform inventoryPanel;
    [SerializeField]
    private RectTransform quickSlotPanel;
    [SerializeField]
    private InventorySO inventory;

    public List<UIInventorySlot> listOfEquipment = new List<UIInventorySlot>(4);
    public List<UIInventorySlot> listOfInventory = new List<UIInventorySlot>(27);
    public List<UIInventorySlot> listOfQuickSlot = new List<UIInventorySlot>(9);

    public Dictionary<int, List<UIInventorySlot>> listOfAllSlots = new Dictionary<int, List<UIInventorySlot>>();

    public void InitAllList()
    {
        for (int i = 0; i < listOfEquipment.Count; i++) 
        {
            UIInventorySlot slot = Instantiate(slotPrefab);
            slot.transform.SetParent(equipmentPanel);
            listOfEquipment.Add(slot);
        }
        for (int i = 0; i < listOfInventory.Count; i++)
        {
            UIInventorySlot slot = Instantiate(slotPrefab);
            slot.transform.SetParent(inventoryPanel);
            listOfInventory.Add(slot);
        }
        for (int i = 0; i < listOfQuickSlot.Count; i++)
        {
            UIInventorySlot slot = Instantiate(slotPrefab);
            slot.transform.SetParent (quickSlotPanel);
            listOfQuickSlot.Add(slot);
        }
        listOfAllSlots.Add(0, listOfEquipment);
        listOfAllSlots.Add(1, listOfInventory);
        listOfAllSlots.Add(2, listOfQuickSlot);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
