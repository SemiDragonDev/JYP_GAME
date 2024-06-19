using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggingSlot : MonoBehaviour
{
    public Image draggingItemImage;
    public TextMeshProUGUI draggingItemCountText;

    public InventoryItem DraggingItem { get; set; }

    void Start()
    {
        draggingItemImage.enabled = false;
    }

    void Update()
    {
        if (draggingItemImage.enabled)
        {
            Vector3 position = Input.mousePosition;
            draggingItemImage.transform.position = position;
            draggingItemCountText.transform.position = position;
        }
    }
}
