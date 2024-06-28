using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : Singleton<Inventory>
{
    public event Action OnInventoryChanged;

    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<CraftingSlot> craftingSlots = new List<CraftingSlot>();
    public BuildSlot buildSlot;
    public static List<QuickSlot> quickSlotsGroup1 = new List<QuickSlot>();
    public static List<QuickSlot> quickSlotsGroup2 = new List<QuickSlot>();
    public DraggingSlot draggingSlot;

    public bool IsDraggingSlot { get; set; } = false;

    private void Start()
    {
        var foundSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();   //  Resources.FindObjectsOfTypeAll�� ��� Active�� �ƴ� ������Ʈ���� ã�ƿ� �� �ִ�. (������ Asset ������ �ִ� ������Ʈ���� ã�ƿ��Ƿ�, prefab�� ������� ��� ������ �ȸ´� ���� ����)
        var sortedSlots = foundSlots.OrderBy(slot => slot.slotIndex).ToList();
        slots = sortedSlots;
        foreach (var slot in slots)
        {
            slot.InventoryItem = null;
        }
        var foundCraftingSlots = Resources.FindObjectsOfTypeAll<CraftingSlot>();
        var sortedCraftingSlots = foundCraftingSlots.OrderBy(slot => slot.slotIndex).ToList();
        craftingSlots = sortedCraftingSlots;
        foreach(var craftingSlot in craftingSlots)
        {
            craftingSlot.InventoryItem = null;
        }
        
        draggingSlot = GameObject.Find("DraggingSlot").GetComponent<DraggingSlot>();

        QuickSlot[] foundQuickSlots = Resources.FindObjectsOfTypeAll<QuickSlot>();    //  QuickSlot�� Index 0~8���� �� ���� �����Ѵ�. ��� ã�ƿ´�.
        Dictionary<int, List<QuickSlot>>  slotGroups = new Dictionary<int, List<QuickSlot>>();  //  index�� ���� �ͳ��� ���� List�� �������� Dict�� ���
        foreach(var quickSlot in foundQuickSlots)
        {
            if (!slotGroups.ContainsKey(quickSlot.slotIndex))   //  index�� ���� key�� ���� ���ٸ�
            {
                slotGroups[quickSlot.slotIndex] = new List<QuickSlot>();    //  �� Ű�� �� ����Ʈ�� �����
            }
            slotGroups[quickSlot.slotIndex].Add(quickSlot); //  �� ����Ʈ�� ������ �߰��Ѵ�
        }
        foreach(var slotIndex in slotGroups.Keys)   //  Key�� 0~8���� ���鼭
        {
            quickSlotsGroup1.Add(slotGroups[slotIndex][0]);
            quickSlotsGroup2.Add(slotGroups[slotIndex][1]); //  �� Ű�� �� ����Ʈ�� ���� �� ���� ������ ���� ����Ʈ�� �����ش�. �׷��� �� ���� Index ������� �����ϱ� �ϼ�!
        }
        foreach (var slot in quickSlotsGroup1)
        {
            slot.QuickSlotItem = null;
        }
        foreach (var slot in quickSlotsGroup2)
        {
            slot.QuickSlotItem = null;
        }
    }

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, count);
                Debug.Log($"Added {item.itemName} to slot {i}"); // �����
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].InventoryItem.item == item && item.isStackable)
            {
                slots[i].InventoryItem.itemCount += count;
                Debug.Log($"Stacked {item.itemName} in slot {i}"); // �����
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning("No empty slots available!"); // ������ ���� ��� ��� �޽��� �߰�
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        slots[slotIndex].ClearSlot();
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Ŭ���� �κ��丮 ������ �巡����������� �Ű��ش�
    /// </summary>
    /// <param name="clickedSlot"> Ŭ���� ���� </param>
    public void ToDraggingItem(int slotIndex)
    {
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        Debug.Log("Dragging Slot�� ����ִ� Item : " + draggingSlot.DraggingItem.item.itemName);
        slots[slotIndex].InventoryItem = null;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Ŭ���� BuildSlot�� dragging ���������� �ű��.
    /// </summary>
    public void BuildToDragging()
    {
        if (draggingSlot.DraggingItem == null)
        {
            draggingSlot.DraggingItem = buildSlot.BuildItem;
            Debug.Log("Dragging Slot�� ����ִ� Item : " + draggingSlot.DraggingItem.item.itemName);
            IsDraggingSlot = true;
            buildSlot.BuildItem = null;

            foreach(var slot in craftingSlots)
            {
                slot.InventoryItem = null;
            }

            OnInventoryChanged?.Invoke();
        }
    }

    /// <summary>
    /// Dragging ���� �������� �� �κ��丮 ���Կ� ���´�
    /// </summary>
    /// <param name="slotIndex"> Ŭ���� �κ��丮 ������ �ε��� </param>
    public void DraggingItemToEmptySlot(int slotIndex)
    {
        slots[slotIndex].InventoryItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = null;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Dragging ���� �����۰� �κ��丮 ���Կ� �ִ� �������� �¹ٲ۴�
    /// </summary>
    /// <param name="slotIndex"> Ŭ���� �κ��丮 ������ �ε��� </param>
    public void SwapWithDraggingItem(int slotIndex)
    {
        var tempItemSave = new InventoryItem(null, 0);
        tempItemSave = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        slots[slotIndex].InventoryItem = tempItemSave;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Dragging ���� �������� Crafing ���Կ� ���´�
    /// </summary>
    /// <param name="slotIndex"> Ŭ���� Crafting ������ �ε��� </param>
    public void DraggingToCrafting(int slotIndex)
    {
        if (draggingSlot.DraggingItem == null)
        {
            Debug.LogWarning("No item is being dragged.");
            return;
        }

        if (craftingSlots[slotIndex].InventoryItem == null)
        {
            craftingSlots[slotIndex].InventoryItem = new InventoryItem(draggingSlot.DraggingItem.item, 1);
        }
        else if (craftingSlots[slotIndex].InventoryItem.item == draggingSlot.DraggingItem.item && draggingSlot.DraggingItem.item.isStackable)
        {
            craftingSlots[slotIndex].InventoryItem.itemCount++;
        }
        else
        {
            Debug.LogWarning("Crafting slot already contains a different item.");
            return;
        }

        draggingSlot.DraggingItem.itemCount--;

        if (draggingSlot.DraggingItem.itemCount <= 0)
        {
            draggingSlot.DraggingItem = null;
            Inventory.Instance.IsDraggingSlot = false;
        }

        OnInventoryChanged?.Invoke();
    }

    public void QSToDraggingFromGroup1(int slotIndex)
    {
        Debug.Log($"QSToDraggingFromGroup1 called with slotIndex: {slotIndex}");

        if (slotIndex < 0 || slotIndex >= quickSlotsGroup1.Count)
        {
            Debug.LogError("Invalid slotIndex for quickSlotsGroup1");
            return;
        }

        if (quickSlotsGroup1[slotIndex].QuickSlotItem != null)
        {
            draggingSlot.DraggingItem = quickSlotsGroup1[slotIndex].QuickSlotItem;
            quickSlotsGroup1[slotIndex].QuickSlotItem = null;
            quickSlotsGroup2[slotIndex].QuickSlotItem = null;
            IsDraggingSlot = true;
            OnInventoryChanged?.Invoke();
            Debug.Log($"DraggingItem set from Group1. slotIndex: {slotIndex}, Item: {draggingSlot.DraggingItem.item.itemName}");
        }
        else
        {
            Debug.LogWarning($"No item to drag in quickSlotsGroup1 at slotIndex: {slotIndex}");
        }
    }

    public void QSToDraggingFromGroup2(int slotIndex)
    {
        Debug.Log($"QSToDraggingFromGroup2 called with slotIndex: {slotIndex}");

        if (slotIndex < 0 || slotIndex >= quickSlotsGroup2.Count)
        {
            Debug.LogError("Invalid slotIndex for quickSlotsGroup2");
            return;
        }

        if (quickSlotsGroup2[slotIndex].QuickSlotItem != null)
        {
            draggingSlot.DraggingItem = quickSlotsGroup2[slotIndex].QuickSlotItem;
            quickSlotsGroup2[slotIndex].QuickSlotItem = null;
            quickSlotsGroup1[slotIndex].QuickSlotItem = null;
            IsDraggingSlot = true;
            OnInventoryChanged?.Invoke();
            Debug.Log($"DraggingItem set from Group2. slotIndex: {slotIndex}, Item: {draggingSlot.DraggingItem.item.itemName}");
        }
        else
        {
            Debug.LogWarning($"No item to drag in quickSlotsGroup2 at slotIndex: {slotIndex}");
        }
    }



    /// <summary>
    /// DraggingSlot �� �ִ� �������� Ŭ���� QuickSlotInven���� �ű��
    /// </summary>
    /// <param name="slotIndex"></param>
    public void DraggingToQS(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= quickSlotsGroup1.Count)
        {
            Debug.LogError("Invalid slotIndex for quickSlots");
            return;
        }

        if (draggingSlot.DraggingItem != null)
        {
            quickSlotsGroup1[slotIndex].QuickSlotItem = draggingSlot.DraggingItem;
            quickSlotsGroup2[slotIndex].QuickSlotItem = draggingSlot.DraggingItem;

            Debug.Log($"QuickSlot Group1 Index {slotIndex} set to {quickSlotsGroup1[slotIndex].QuickSlotItem.item.itemName}");
            Debug.Log($"QuickSlot Group2 Index {slotIndex} set to {quickSlotsGroup2[slotIndex].QuickSlotItem.item.itemName}");

            draggingSlot.DraggingItem = null;
            IsDraggingSlot = false;
            OnInventoryChanged?.Invoke();

            // �߰� ����� �α�
            Debug.Log($"Post-OnInventoryChanged status: Group1 Index {slotIndex}, QuickSlotItem: {(quickSlotsGroup1[slotIndex].QuickSlotItem != null ? quickSlotsGroup1[slotIndex].QuickSlotItem.item.itemName : "null")}");
            Debug.Log($"Post-OnInventoryChanged status: Group2 Index {slotIndex}, QuickSlotItem: {(quickSlotsGroup2[slotIndex].QuickSlotItem != null ? quickSlotsGroup2[slotIndex].QuickSlotItem.item.itemName : "null")}");
        }
        else
        {
            Debug.LogWarning("No item is being dragged.");
        }
    }


    public void SwapDraggingAndQS(int slotIndex)
    {
        var tempItem = new InventoryItem(null, 0);
        tempItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = quickSlotsGroup1[slotIndex].QuickSlotItem;
        draggingSlot.DraggingItem = quickSlotsGroup2[slotIndex].QuickSlotItem;
        quickSlotsGroup1[slotIndex].QuickSlotItem = tempItem;
        quickSlotsGroup2[slotIndex].QuickSlotItem = tempItem;
        OnInventoryChanged?.Invoke();
    }


    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

    public List<CraftingSlot> GetCraftingSlots()
    {
        return craftingSlots;
    }

    public List<QuickSlot> GetQuickSlots()
    {
        return quickSlotsGroup1;
    }
}
