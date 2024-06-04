using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (inventoryUI.isInventoryOpen)
        {
            cursorImage.gameObject.SetActive(true);
            Vector2 cursorPos = Input.mousePosition;
            cursorImage.position = cursorPos;
        }
        else
        {
            cursorImage.gameObject.SetActive(false);
        }
    }
}
