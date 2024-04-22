using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnDrag : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private UIInventorySlot inventorySlot;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        cam = Camera.main;
        inventorySlot = GetComponentInChildren<UIInventorySlot>();
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void SetData(Sprite spirte, int amount)
    {
        inventorySlot.SetData(spirte, amount);
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
