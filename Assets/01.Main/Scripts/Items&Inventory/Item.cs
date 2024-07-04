using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public bool isStackable;
    public int maxStackSize = 64;
    public bool isConsumable;
    public bool isEquipable;
    public GameObject itemPrefab;

    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;

    public void ApplyTransform(Transform targetTransform)
    {
        targetTransform.localPosition = localPosition;
        targetTransform.localRotation = localRotation;
        targetTransform.localScale = localScale;
    }
}
