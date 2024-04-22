using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO item;

    [field: SerializeField]
    public int amount { get; set; } = 1;
}
