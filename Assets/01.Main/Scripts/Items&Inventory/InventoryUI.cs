using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Image[] slotImages; // 27���� ���� �̹����� ��� �迭

    [SerializeField]
    private TextMeshProUGUI[] slotTexts; // 27���� ���� �ؽ�Ʈ�� ��� �迭

    [SerializeField]
    private GameObject inventoryUI; // �κ��丮 UI ��ü�� ��� GameObject

    [SerializeField]
    private SetMouseState setMouseState;

    [SerializeField]
    private DraggingSlot draggingSlot;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;

    public bool IsInventoryOpen { get; private set; } // �κ��丮�� ���ȴ��� �������� Ȯ���� �� �ִ� bool ����

    void Start()
    {
        inventory.OnInventoryChanged += UpdateUI;

        // �ʱ⿡�� �κ��丮 UI�� ��Ȱ��ȭ
        inventoryUI.SetActive(false);
        IsInventoryOpen = false;

        // �÷��̾��� Animator�� PlayerMovement ������Ʈ�� ã�Ƽ� �Ҵ�
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
            // UI�� ���� �� �÷��̾��� Animator�� PlayerMovement�� ��Ȱ��ȭ
            playerAnimator.enabled = false;
            playerMovement.enabled = false;
            setMouseState.UnlockCursor();
        }
        else
        {
            // UI�� ���� �� �÷��̾��� Animator�� PlayerMovement�� �ٽ� Ȱ��ȭ
            playerAnimator.enabled = true;
            playerMovement.enabled = true;
            setMouseState.LockCursor();
        }
    }

    void UpdateUI()
    {
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
            draggingSlot.draggingItemImage.enabled = true;
        }
    }
}
