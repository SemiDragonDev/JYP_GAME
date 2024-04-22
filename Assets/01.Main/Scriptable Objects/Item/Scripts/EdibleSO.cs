using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Edible_", menuName = "ItemSO/Edible")]
public class EdibleSO : ItemSO, IDestroyable, IItemAction
{
    [SerializeField]
    private List<ModifierData> modifiersData = new List<ModifierData>();

    public string ActionName => "Consume";

    public bool PerformAction(GameObject character)
    {
        foreach (ModifierData data in modifiersData)
        {
            data.statModifier.AffectCharacter(character, data.value);
        }
        return true;
    }

    private void Awake()
    {
        type = ItemType.Edible;
    }
}

public interface IDestroyable
{

}

public interface IItemAction
{
    public string ActionName { get; }
    bool PerformAction(GameObject character);
}

[Serializable]

public class ModifierData
{
    public CharacterStatModifierSO statModifier;
    public float value;
}