using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorFollow : MonoBehaviour
{
    [SerializeField] private RectTransform cursorImage;
    [SerializeField] InventoryUI inventoryUI;

    private void Start()
    {
        cursorImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inventoryUI.IsInventoryOpen)
        {
            cursorImage.gameObject.SetActive(true);
            Vector2 cursorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorImage.parent as RectTransform, Input.mousePosition, null, out cursorPos);

            cursorImage.localPosition = cursorPos;
        }
        else
        {
            cursorImage.gameObject.SetActive(false);
        }
    }
}
