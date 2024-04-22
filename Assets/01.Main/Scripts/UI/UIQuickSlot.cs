using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickSlot : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot slotPrefab;
    [SerializeField]
    private RectTransform quickSlotPanel;

    public int QuickSlotSize = 9;

    List<UIInventorySlot> listOfQuickSlotUI = new List<UIInventorySlot>();

    public void InitQuickSlotUI(int QuickSlotSize)
    {
        for(int i=0; i<QuickSlotSize; i++)
        {
            UIInventorySlot quickSlot = Instantiate(slotPrefab);
            quickSlot.transform.SetParent(quickSlotPanel);
            listOfQuickSlotUI.Add(quickSlot);
        }
    }
}
