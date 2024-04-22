using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text amountTxt;
    [SerializeField]
    private InventorySO inventory;

    private Sprite sprite;
    private int amount;

    public event Action<UIInventorySlot> OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag;

    private bool empty = true;

    private void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }
    public void SetData(Sprite sprite, int amount)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.amountTxt.text = amount + "";
        empty = false;
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
