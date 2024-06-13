using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리 슬롯")]
    public Transform itemsParent;
    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab;
    public int slotCount = 27;
    public bool isInventoryOpen = false;

    private Inventory inventory;
    private InventorySlot[] slots;


    [Space(10)]
    [Header("크래프팅 슬롯")]
    public Transform craftingParent;
    public GameObject craftingSlotPrefab;
    public int craftingSlotCount = 4;

    private CraftingSlot[] craftingSlots;


    private PlayerMovement playerMovement;
    private SetMouseState mouseState;

    void Start()
    {
        mouseState = FindObjectOfType<SetMouseState>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        inventory = Inventory.Instance;
        inventory.onInventoryChangedCallback += UpdateUI;

        slots = new InventorySlot[slotCount];
        craftingSlots = new CraftingSlot[craftingSlotCount];

        // 동적으로 인벤토리 슬롯 생성
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, itemsParent);
            slots[i] = slotGO.GetComponent<InventorySlot>();
        }

        for (int i = 0; i < craftingSlots.Length; i++)
        {
            GameObject craftingSlotGO = Instantiate(craftingSlotPrefab, craftingParent);
            craftingSlots[i] = craftingSlotGO.GetComponent<CraftingSlot>();
            craftingSlots[i].slotIndex = i;
        }

        UpdateUI();  // Initial UI update
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryUI.SetActive(isInventoryOpen);

            if (isInventoryOpen)
            {
                playerMovement.enabled = false;
                mouseState.UnlockCursor();
            }
            else
            {
                playerMovement.enabled= true;
                mouseState.LockCursor();
            }
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
