using UnityEngine;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public InventoryItem inventoryItem = null; // ��������� null�� �ʱ�ȭ

    public void AddItem(Item newItem, int count)
    {
        inventoryItem = new InventoryItem(newItem, count);
        Debug.Log($"Added {newItem.itemName} with count {count} to InventorySlot"); // ����� �޽��� �߰�
    }

    public void ClearSlot()
    {
        inventoryItem = null;
    }

    public bool IsEmpty()
    {
        bool empty = inventoryItem == null;
        Debug.Log($"IsEmpty called, result: {empty}"); // ����� �޽��� �߰�
        return empty;
    }
}
