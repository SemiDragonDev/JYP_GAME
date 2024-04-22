using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Resource,
    Equipment,
    Edible,
    Default
}
public abstract class ItemSO : ScriptableObject
{
    [field:SerializeField]
    public bool IsStackable { get; set; }
    [field: SerializeField]
    public int MaxStackSize { get; set; } = 1;
    
    public Sprite ItemImage;
    public ItemType type;

    public int ID => GetInstanceID();
}
