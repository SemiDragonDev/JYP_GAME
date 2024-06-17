using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggingSlot : MonoBehaviour
{
    public static InventoryItem DraggingItem { get; set; }

    public static Image draggingItemImage;
    public static TextMeshProUGUI draggingItemCount;

    private void Start()
    {
        DraggingItem = new InventoryItem(null, 0);
        draggingItemImage = GetComponent<Image>();
        draggingItemCount = GetComponentInChildren<TextMeshProUGUI>();
    }
}
