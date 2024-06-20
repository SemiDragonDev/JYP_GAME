using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public RectTransform draggingImageRect;
    public Image draggingImage;
    public TextMeshProUGUI draggingText;
}
