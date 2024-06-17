using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggingSlot : MonoBehaviour
{
    public InventoryItem DraggingItem { get; set; }

    public Image draggingItemImage;
    public TextMeshProUGUI draggingItemCount;
}
