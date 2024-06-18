using UnityEngine;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public InventoryItem inventoryItem = null; // 명시적으로 null로 초기화

    public void AddItem(Item newItem, int count)
    {
        inventoryItem = new InventoryItem(newItem, count);
        Debug.Log($"Added {newItem.itemName} with count {count} to InventorySlot"); // 디버그 메시지 추가
    }

    public void ClearSlot()
    {
        inventoryItem = null;
    }

    public bool IsEmpty()
    {
        bool empty = inventoryItem == null;
        Debug.Log($"IsEmpty called, result: {empty}"); // 디버그 메시지 추가
        return empty;
    }
}
