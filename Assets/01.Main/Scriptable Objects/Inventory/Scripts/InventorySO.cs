using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory_", menuName = "InventorySO")]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventorySlot> Container;

    [field: SerializeField]
    public int InventorySize { get; private set; } = 27;

    public event Action<Dictionary<int, InventorySlot>> OnInventoryChanged;

    public void InitContainer()
    {
        Container = new List<InventorySlot>();
        for (int i = 0; i < InventorySize; i++) 
        {
            Container.Add(InventorySlot.GetEmptySlot());
        }
    }

    public int AddItem(ItemSO item, int amount)
    {
        if(item.IsStackable == false)
        {
            for (int i = 0; i < Container.Count; i++)
            {
                while (amount > 0 && IsInventoryFull() == false)
                {
                    amount -= AddItemToFirstFreeSlot(item, 1);
                }
            }
            InformAboutChange();
            return amount;
        }
        amount = AddStackableItem(item, amount);
        InformAboutChange();
        return amount;
    }

    private int AddItemToFirstFreeSlot(ItemSO item, int amount)
    {
        InventorySlot newItem = new InventorySlot
        {
            item = item,
            amount = amount
        };

        for (int i = 0; i < Container.Count; i++) 
        {
            if (Container[i].IsEmpty)
            {
                Container[i] = newItem;
                return amount;
            }
        }
        return 0;
    }

    private bool IsInventoryFull()
    {
        for(int i=0; i < Container.Count; i++)
        {
            if (Container[i].IsEmpty)
                return false;
        }
        return true;
    }

    private int AddStackableItem(ItemSO item, int amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].IsEmpty) 
                continue;
            if (Container[i].item.ID == item.ID) 
            {
                int amountPossibleToTake = Container[i].item.MaxStackSize - Container[i].amount;

                if (amount > amountPossibleToTake) 
                {
                    Container[i] = Container[i].ChangeAmount(Container[i].item.MaxStackSize);
                    amount -= amountPossibleToTake;
                }
                else
                {
                    Container[i] = Container[i].ChangeAmount(Container[i].amount + amount);
                    InformAboutChange();
                    return 0;
                }
            }
        }
        while (amount > 0 && IsInventoryFull() == false) 
        {
            int newAmount = Mathf.Clamp(amount, 0, item.MaxStackSize);
            amount -= newAmount;
            AddItemToFirstFreeSlot(item, newAmount);
        }
        return amount;
    }

    public void AddItem(InventorySlot item)
    {
        AddItem(item.item, item.amount);
    }

    public Dictionary<int, InventorySlot> GetCurrentInventoryState()
    {
        Dictionary<int, InventorySlot> returnValue = new Dictionary<int, InventorySlot>();
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].IsEmpty)
                continue;
            returnValue[i] = Container[i];
        }
        return returnValue;
    }

    public InventorySlot GetItemAt(int itemIndex)
    {
        return Container[itemIndex];
    }

    public void SwapItem(int itemIndex_1, int itemIndex2)
    {
        InventorySlot item1 = Container[itemIndex_1];
        Container[itemIndex_1] = Container[itemIndex2];
        Container[itemIndex2] = item1;
        InformAboutChange();
    }

    private void InformAboutChange()
    {
        OnInventoryChanged?.Invoke(GetCurrentInventoryState());
    }
}

[Serializable]
public struct InventorySlot
{
    public int amount;
    public ItemSO item;
    public bool IsEmpty => item == null;

    public InventorySlot ChangeAmount(int newAmount)
    {
        return new InventorySlot
        {
            item = this.item,
            amount = newAmount
        };
    }

    public static InventorySlot GetEmptySlot() => new InventorySlot
    {
        item = null,
        amount = 0
    };
}