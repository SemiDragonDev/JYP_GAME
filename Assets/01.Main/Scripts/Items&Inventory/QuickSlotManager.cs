using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public List<QuickSlot> quickSlots;
    private int currentSlotIndex = 0;
    public Transform handTransform;
    public StringBuilder sb = new StringBuilder();

    void Start()
    {
        // �ʱ� ����
        UpdateSlotUI();
    }

    void Update()
    {
        // ���콺 �� �Է� ó��
        HandleMouseScroll();
    }

    void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0f) // �� �ٿ�
        {
            currentSlotIndex = (currentSlotIndex + 1) % quickSlots.Count;
            UpdateSlotUI();
        }
        else if (scroll > 0f) // �� ��
        {
            currentSlotIndex = (currentSlotIndex - 1 + quickSlots.Count) % quickSlots.Count;
            UpdateSlotUI();
        }
    }


    void UpdateSlotUI()
    {
        // ���õ� ���� ���� ǥ��
        for (int i = 0; i < quickSlots.Count; i++)
        {
            quickSlots[i].Highlight(i == currentSlotIndex);
        }

        // ���õ� ������ ������ ���� ��� �� �������� �� ���� ���
        InventoryItem selectedItem = quickSlots[currentSlotIndex].QuickSlotItem;
        if (selectedItem != null)
        {
            Debug.Log($"���õ� ����: {currentSlotIndex}, ������: {selectedItem.item.itemName}");
            EquipItemInHand(selectedItem.item);
        }
        else
        {
            Debug.Log($"���õ� ����: {currentSlotIndex}, ������ ����");
            UnequipItemInHand();
        }
    }

    private void EquipItemInHand(Item item)
    {
        // ������ �տ� ��� �ִ� ������ ����
        UnequipItemInHand();

        // ���ο� ������ �ν��Ͻ�ȭ�Ͽ� �� ���� ��ġ
        if (item != null)
        {
            sb.Append(item.itemName);
            var itemToInstantiate = ObjectPool.Instance.GetPooledObject(sb.ToString());
            sb.Clear();
            itemToInstantiate.gameObject.transform.SetParent(handTransform, false);
            item.ApplyTransform(itemToInstantiate.transform);
        }
    }

    private void UnequipItemInHand()
    {
        // �� ���� ��� �ִ� ������ ����
        foreach (Transform child in handTransform)
        {
            ObjectPool.Instance.ReturnToPool(child.GetComponent<PooledObject>());
        }
    }
}
