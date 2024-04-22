using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment_", menuName = "ItemSO/Equipment")]
public class EquipmentSO : ItemSO
{
    private void Awake()
    {
        type = ItemType.Equipment;
    }
}
