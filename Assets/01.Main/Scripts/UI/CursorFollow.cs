using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    [SerializeField] private RectTransform cursorImage;
    [SerializeField] InventoryUI inventoryUI;
    Vector2 posCorrection;
    [SerializeField] float posCorrectionX = 20f;
    [SerializeField] float posCorrectionY = -20f;

    private void Start()
    {
        cursorImage.gameObject.SetActive(false);
        posCorrection = new Vector2(posCorrectionX, posCorrectionY);
    }

    private void Update()
    {
        if (inventoryUI.isInventoryOpen)
        {
            cursorImage.gameObject.SetActive(true);
            Vector2 cursorPos = Input.mousePosition;
            cursorImage.position = cursorPos + posCorrection;
        }
        else
        {
            cursorImage.gameObject.SetActive(false);
        }
    }
}
