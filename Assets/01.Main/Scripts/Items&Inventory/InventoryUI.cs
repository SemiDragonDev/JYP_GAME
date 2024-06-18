using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Image[] slotImages; // 27���� ���� �̹����� ��� �迭

    [SerializeField]
    private Text[] slotTexts; // 27���� ���� �ؽ�Ʈ�� ��� �迭

    [SerializeField]
    private GameObject inventoryUI; // �κ��丮 UI ��ü�� ��� GameObject

    [SerializeField]
    private SetMouseState setMouseState;

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
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty())
            {
                slotImages[i].sprite = slots[i].inventoryItem.item.itemImage;
                slotTexts[i].text = slots[i].inventoryItem.itemCount.ToString();
                slotImages[i].enabled = true;

                // ���� Ŭ�� �̺�Ʈ �߰�
                int index = i; // Ŭ���� ���� �ذ��� ���� �ӽ� ���� ���
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
