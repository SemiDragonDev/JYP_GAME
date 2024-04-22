using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource_", menuName = "ItemSO/Resource")]
public class ResourceSO : ItemSO
{
    private void Awake()
    {
        type = ItemType.Resource;
    }
}
