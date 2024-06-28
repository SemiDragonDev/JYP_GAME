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
        var foundSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();   //  Resources.FindObjectsOfTypeAll을 써야 Active가 아닌 오브젝트까지 찾아올 수 있다. (하지만 Asset 폴더에 있는 오브젝트까지 찾아오므로, prefab을 만들었을 경우 개수가 안맞는 일이 생김)
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

        QuickSlot[] foundQuickSlots = Resources.FindObjectsOfTypeAll<QuickSlot>();    //  QuickSlot은 Index 0~8까지 두 쌍이 존재한다. 모두 찾아온다.
        Dictionary<int, List<QuickSlot>>  slotGroups = new Dictionary<int, List<QuickSlot>>();  //  index가 같은 것끼리 같은 List로 묶기위해 Dict를 사용
        foreach(var quickSlot in foundQuickSlots)
        {
            if (!slotGroups.ContainsKey(quickSlot.slotIndex))   //  index를 가진 key가 아직 없다면
            {
                slotGroups[quickSlot.slotIndex] = new List<QuickSlot>();    //  그 키에 새 리스트를 만들고
            }
            slotGroups[quickSlot.slotIndex].Add(quickSlot); //  그 리스트에 슬롯을 추가한다
        }
        foreach(var slotIndex in slotGroups.Keys)   //  Key를 0~8까지 돌면서
        {
            quickSlotsGroup1.Add(slotGroups[slotIndex][0]);
            quickSlotsGroup2.Add(slotGroups[slotIndex][1]); //  각 키에 들어간 리스트에 있을 두 쌍의 슬롯을 각각 리스트로 나눠준다. 그러면 각 쌍을 Index 순서대로 정렬하기 완성!
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
                Debug.Log($"Added {item.itemName} to slot {i}"); // 디버그
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].InventoryItem.item == item && item.isStackable)
            {
                slots[i].InventoryItem.itemCount += count;
                Debug.Log($"Stacked {item.itemName} in slot {i}"); // 디버그
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning("No empty slots available!"); // 슬롯이 없는 경우 경고 메시지 추가
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        slots[slotIndex].ClearSlot();
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 클릭한 인벤토리 슬롯을 드래깅아이템으로 옮겨준다
    /// </summary>
    /// <param name="clickedSlot"> 클릭한 슬롯 </param>
    public void ToDraggingItem(int slotIndex)
    {
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        Debug.Log("Dragging Slot에 들어있는 Item : " + draggingSlot.DraggingItem.item.itemName);
        slots[slotIndex].InventoryItem = null;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 클릭한 BuildSlot을 dragging 아이템으로 옮긴다.
    /// </summary>
    public void BuildToDragging()
    {
        if (draggingSlot.DraggingItem == null)
        {
            draggingSlot.DraggingItem = buildSlot.BuildItem;
            Debug.Log("Dragging Slot에 들어있는 Item : " + draggingSlot.DraggingItem.item.itemName);
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
    /// Dragging 중인 아이템을 빈 인벤토리 슬롯에 놓는다
    /// </summary>
    /// <param name="slotIndex"> 클릭한 인벤토리 슬롯의 인덱스 </param>
    public void DraggingItemToEmptySlot(int slotIndex)
    {
        slots[slotIndex].InventoryItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = null;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Dragging 중인 아이템과 인벤토리 슬롯에 있는 아이템을 맞바꾼다
    /// </summary>
    /// <param name="slotIndex"> 클릭한 인벤토리 슬롯의 인덱스 </param>
    public void SwapWithDraggingItem(int slotIndex)
    {
        var tempItemSave = new InventoryItem(null, 0);
        tempItemSave = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = slots[slotIndex].InventoryItem;
        slots[slotIndex].InventoryItem = tempItemSave;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Dragging 중인 아이템을 Crafing 슬롯에 놓는다
    /// </summary>
    /// <param name="slotIndex"> 클릭한 Crafting 슬롯의 인덱스 </param>
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
    /// DraggingSlot 에 있는 아이템을 클릭한 QuickSlotInven으로 옮긴다
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

            // 추가 디버깅 로그
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
