using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public int maxStackSize = 64;
    public bool isConsumable;
    public bool isEquipable;
    public GameObject itemPrefab;
}
