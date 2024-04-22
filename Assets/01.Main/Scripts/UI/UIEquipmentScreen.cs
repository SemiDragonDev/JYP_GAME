using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentScreen : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot slotPrefab;
    [SerializeField]
    private RectTransform equipmentSlotPanel;

    public int EquipmentSlotSize = 4;

    List<UIInventorySlot> listOfEquipSlotUI = new List<UIInventorySlot>();

    public void InitEquipSlotUI(int EquipSlotSize)
    {
        for (int i = 0; i < EquipSlotSize; i++)
        {
            UIInventorySlot quickSlot = Instantiate(slotPrefab);
            quickSlot.transform.SetParent(equipmentSlotPanel);
            listOfEquipSlotUI.Add(quickSlot);
        }
    }
}
