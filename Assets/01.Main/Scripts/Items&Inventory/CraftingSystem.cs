using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public RecipeDatabase recipeDatabase;
    public CraftingSlot[] craftingSlots;
    public BuildSlot buildSlot;
    public EmptyItem emptyItem;

    private List<Item> currentItems = new List<Item>();

    //  ũ������ ������ ���°� ����� �� ȣ��Ǵ� �޼���
    public void OnCraftingChanged()
    {
        currentItems.Clear();

        foreach (var slot in craftingSlots)
        {
            if (slot.InventoryItem != null)
            {
                currentItems.Add(slot.InventoryItem.item);
            }
            else
            {
                currentItems.Add(emptyItem);
            }
        }

        Recipe matchedRecipe = FindMatchingRecipe(currentItems);
        if (matchedRecipe != null)
        {
            buildSlot.BuildItem = new InventoryItem(matchedRecipe.result, 1);
            buildSlot.DisplayItem(matchedRecipe.result);
        }
        else
        {
            buildSlot.ClearSlot();
        }
    }


    private Recipe FindMatchingRecipe(List<Item> items)
    {
        foreach (var recipe in recipeDatabase.recipes)
        {
            if (recipe.items.Count == items.Count) // ���� ����Ʈ�� ũ�Ⱑ ������ Ȯ��
            {
                bool allItemsMatch = true;
                for (int i = 0; i < items.Count; i++)
                {
                    if (recipe.items[i] != items[i]) // �� ��ġ�� �������� ��
                    {
                        allItemsMatch = false; // �ϳ��� �ٸ��� false
                        break;
                    }
                }
                if (allItemsMatch)
                {
                    return recipe; // ��� �������� ��ġ�ϸ� �ش� �����Ǹ� ��ȯ
                }
            }
        }
        return null; // ��Ī�Ǵ� �����ǰ� ���ٸ� null ��ȯ
    }
}
