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
        // 초기 설정
        UpdateSlotUI();
    }

    void Update()
    {
        // 마우스 휠 입력 처리
        HandleMouseScroll();
    }

    void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0f) // 휠 다운
        {
            currentSlotIndex = (currentSlotIndex + 1) % quickSlots.Count;
            UpdateSlotUI();
        }
        else if (scroll > 0f) // 휠 업
        {
            currentSlotIndex = (currentSlotIndex - 1 + quickSlots.Count) % quickSlots.Count;
            UpdateSlotUI();
        }
    }


    void UpdateSlotUI()
    {
        // 선택된 슬롯 강조 표시
        for (int i = 0; i < quickSlots.Count; i++)
        {
            quickSlots[i].Highlight(i == currentSlotIndex);
        }

        // 선택된 슬롯의 아이템 정보 출력 및 아이템을 손 끝에 들기
        InventoryItem selectedItem = quickSlots[currentSlotIndex].QuickSlotItem;
        if (selectedItem != null)
        {
            Debug.Log($"선택된 슬롯: {currentSlotIndex}, 아이템: {selectedItem.item.itemName}");
            EquipItemInHand(selectedItem.item);
        }
        else
        {
            Debug.Log($"선택된 슬롯: {currentSlotIndex}, 아이템 없음");
            UnequipItemInHand();
        }
    }

    private void EquipItemInHand(Item item)
    {
        // 기존에 손에 들고 있는 아이템 제거
        UnequipItemInHand();

        // 새로운 아이템 인스턴스화하여 손 끝에 배치
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
        // 손 끝에 들고 있는 아이템 제거
        foreach (Transform child in handTransform)
        {
            ObjectPool.Instance.ReturnToPool(child.GetComponent<PooledObject>());
        }
    }
}
