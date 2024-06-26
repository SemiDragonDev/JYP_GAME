using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [Header("인벤토리 슬롯")]
    [SerializeField]
    private Image[] slotImages; // 27개의 슬롯 이미지를 담는 배열

    [SerializeField]
    private TextMeshProUGUI[] slotTexts; // 27개의 슬롯 텍스트를 담는 배열

    [Header("크래프팅 슬롯")]
    [SerializeField]
    private Image[] craftingSlotImages;

    [SerializeField]
    private TextMeshProUGUI[] craftingSlotTexts;

    [Header("빌드 슬롯")]
    [SerializeField]
    private Image buildSlotImage;

    [SerializeField]
    private BuildSlot buildSlot;

    [Space(10)]
    [SerializeField]
    private GameObject inventoryUI; // 인벤토리 UI 전체를 담는 GameObject

    [SerializeField]
    private SetMouseState setMouseState;

    [SerializeField]
    private DraggingSlot draggingSlot;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;

    public bool IsInventoryOpen { get; private set; } // 인벤토리가 열렸는지 닫혔는지 확인할 수 있는 bool 변수

    void Start()
    {
        inventory.OnInventoryChanged += UpdateUI;

        // 초기에는 인벤토리 UI를 비활성화
        inventoryUI.SetActive(false);
        IsInventoryOpen = false;

        // 플레이어의 Animator와 PlayerMovement 컴포넌트를 찾아서 할당
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnDestroy()
    {
        inventory.OnInventoryChanged -= UpdateUI;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryUI();
        }
    }

    void ToggleInventoryUI()
    {
        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);
        IsInventoryOpen = !isActive;

        if (!isActive)
        {
            // UI가 켜질 때 플레이어의 Animator와 PlayerMovement를 비활성화
            playerAnimator.enabled = false;
            playerMovement.enabled = false;
            setMouseState.UnlockCursor();
        }
        else
        {
            // UI가 꺼질 때 플레이어의 Animator와 PlayerMovement를 다시 활성화
            playerAnimator.enabled = true;
            playerMovement.enabled = true;
            setMouseState.LockCursor();
        }
    }

    void UpdateUI()
    {
        //  InventorySlot UI 업데이트
        var slots = inventory.GetSlots();
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty())
            {
                slotImages[i].sprite = slots[i].InventoryItem.item.itemImage;
                slotImages[i].color = Color.white;
                if (slots[i].InventoryItem.itemCount > 1)
                {
                    slotTexts[i].text = slots[i].InventoryItem.itemCount.ToString();
                }
                else
                {
                    slotTexts[i].text = "";
                }
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = Color.clear;
                slotTexts[i].text = "";
            }
        }

        //  DraggingSlot UI 업데이트
        if (draggingSlot.DraggingItem == null)
        {
            draggingSlot.draggingItemImage.color = Color.clear;
            draggingSlot.draggingItemCountText.text = "";
            draggingSlot.draggingItemImage.enabled = false;
        }
        else
        {
            draggingSlot.draggingItemImage.sprite = draggingSlot.DraggingItem.item.itemImage;
            draggingSlot.draggingItemImage.color = Color.white;
            if (draggingSlot.DraggingItem.itemCount > 1)
            {
                draggingSlot.draggingItemCountText.text = draggingSlot.DraggingItem.itemCount.ToString();
            }
            else
            {
                draggingSlot.draggingItemCountText.text = "";
            }
            draggingSlot.draggingItemImage.enabled = true;
        }

        //  CraftingSlot UI 업데이트
        var craftingSlots = inventory.GetCraftingSlots();
        for (int i = 0; i < craftingSlots.Count; i++)
        {
            if (!craftingSlots[i].IsEmpty())
            {
                craftingSlotImages[i].sprite = craftingSlots[i].InventoryItem.item.itemImage;
                craftingSlotImages[i].color = Color.white;
                if (craftingSlots[i].InventoryItem.itemCount > 1)
                {
                    craftingSlotTexts[i].text = craftingSlots[i].InventoryItem.itemCount.ToString();
                }
                else
                {
                    craftingSlotTexts[i].text = "";
                }
            }
            else
            {
                craftingSlotImages[i].sprite = null;
                craftingSlotImages[i].color = Color.clear;
                craftingSlotTexts[i].text = "";
            }
        }

        //  BuildSlot UI 업데이트
        if (buildSlot.IsEmpty())
        {
            buildSlot.ClearSlot();
        }
    }
}
