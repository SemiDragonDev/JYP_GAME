using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public event Action OnInventoryChanged;

    public static List<InventorySlot> slots = new List<InventorySlot>();
    public static List<CraftingSlot> craftingSlots = new List<CraftingSlot>();
    public static BuildSlot buildSlot;
    public static List<QuickSlot> quickSlots;
    public static List<InvenQuickSlot> invenQuickSlots;
    public static DraggingSlot draggingSlot;

    public List<Item> startingItems; // 게임 시작 시 제공할 아이템 리스트

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

        var foundQuickSlots = new List<QuickSlot>(FindObjectsOfType<QuickSlot>());
        var sortedQuickSlots = foundQuickSlots.OrderBy(slot => slot.slotIndex).ToList();
        quickSlots = sortedQuickSlots;
        foreach (var quickSlot in quickSlots)
        {
            quickSlot.QuickSlotItem = null;
        }

        var foundInvenQuickSlots = Resources.FindObjectsOfTypeAll<InvenQuickSlot>();
        var sortedInvenQuickSlots = foundInvenQuickSlots.OrderBy(slots => slots.slotIndex).ToList();
        invenQuickSlots = sortedInvenQuickSlots;
        foreach (var invenQuickSlots in invenQuickSlots)
        {
            invenQuickSlots.InvenQuickSlotItem = null;
        }

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        foreach (var item in startingItems)
        {
            AddItem(item, 1);
        }
    }

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, count);
                Debug.Log($"{item.itemName} 아이템 {i}번 슬롯에 추가"); // 디버그
                OnInventoryChanged?.Invoke();
                return;
            }
            else if (slots[i].InventoryItem.item == item && item.isStackable)
            {
                slots[i].InventoryItem.itemCount += count;
                Debug.Log($"스택 가능 {item.itemName} 아이템 {i}번 슬롯에 추가"); // 디버그
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning("더 이상 빈 슬롯이 없습니다!"); // 슬롯이 없는 경우 경고 메시지 추가
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
            Debug.LogWarning("드래깅 중인 아이템이 없습니다.");
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
            Debug.LogWarning("크래프팅 슬롯에 이미 다른 아이템이 들어있습니다.");
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

    public void QSToDragging(int slotIndex)
    { 
        if (quickSlots[slotIndex].QuickSlotItem != null)
        {
            draggingSlot.DraggingItem = quickSlots[slotIndex].QuickSlotItem;
            quickSlots[slotIndex].QuickSlotItem = null;
            invenQuickSlots[slotIndex].InvenQuickSlotItem = null;
            IsDraggingSlot = true;
            OnInventoryChanged?.Invoke();
        }
    }

    
    /// <summary>
    /// DraggingSlot 에 있는 아이템을 클릭한 QuickSlotInven으로 옮긴다
    /// </summary>
    /// <param name="slotIndex"></param>
    public void DraggingToQS(int slotIndex)
    {
        if (draggingSlot.DraggingItem != null)
        {
            quickSlots[slotIndex].QuickSlotItem = draggingSlot.DraggingItem;
            invenQuickSlots[slotIndex].InvenQuickSlotItem = draggingSlot.DraggingItem;

            draggingSlot.DraggingItem = null;
            IsDraggingSlot = false;
            OnInventoryChanged?.Invoke();
        }
    }

    public void SwapDraggingAndQS(int slotIndex)
    {
        var tempItem = new InventoryItem(null, 0);
        tempItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = quickSlots[slotIndex].QuickSlotItem;
        quickSlots[slotIndex].QuickSlotItem = tempItem;
        invenQuickSlots[slotIndex].InvenQuickSlotItem = tempItem;
        OnInventoryChanged?.Invoke();
    }

    public void IQSToDragging(int slotIndex)
    {
        if (quickSlots[slotIndex].QuickSlotItem != null)
        {
            draggingSlot.DraggingItem = invenQuickSlots[slotIndex].InvenQuickSlotItem;
            quickSlots[slotIndex].QuickSlotItem = null;
            invenQuickSlots[slotIndex].InvenQuickSlotItem = null;
            IsDraggingSlot = true;
            OnInventoryChanged?.Invoke();
        }
    }

    public void DraggingToIQS(int slotIndex)
    {
        if (draggingSlot.DraggingItem != null)
        {
            quickSlots[slotIndex].QuickSlotItem = draggingSlot.DraggingItem;
            invenQuickSlots[slotIndex].InvenQuickSlotItem = draggingSlot.DraggingItem;

            draggingSlot.DraggingItem = null;
            IsDraggingSlot = false;
            OnInventoryChanged?.Invoke();
        }
    }

    public void SwapDraggingAndIQS(int slotIndex)
    {
        var tempItem = new InventoryItem(null, 0);
        tempItem = draggingSlot.DraggingItem;
        draggingSlot.DraggingItem = invenQuickSlots[slotIndex].InvenQuickSlotItem;
        quickSlots[slotIndex].QuickSlotItem = tempItem;
        invenQuickSlots[slotIndex].InvenQuickSlotItem = tempItem;
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
        return quickSlots;
    }
}
