using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Image[] slotImages; // 27개의 슬롯 이미지를 담는 배열

    [SerializeField]
    private Text[] slotTexts; // 27개의 슬롯 텍스트를 담는 배열

    [SerializeField]
    private GameObject inventoryUI; // 인벤토리 UI 전체를 담는 GameObject

    [SerializeField]
    private SetMouseState setMouseState;

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
        var slots = inventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty())
            {
                slotImages[i].sprite = slots[i].inventoryItem.item.itemImage;
                slotTexts[i].text = slots[i].inventoryItem.itemCount.ToString();
                slotImages[i].enabled = true;

                // 슬롯 클릭 이벤트 추가
                int index = i; // 클로저 문제 해결을 위해 임시 변수 사용
                slotImages[i].GetComponent<Button>().onClick.AddListener(() => slots[index].OnSlotClicked());
            }
            else
            {
                slotImages[i].enabled = false;
                slotTexts[i].text = "";
                slotImages[i].GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
    }
}
