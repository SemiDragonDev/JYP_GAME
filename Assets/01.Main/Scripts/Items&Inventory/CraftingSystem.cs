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

    //  크래프팅 슬롯의 상태가 변경될 때 호출되는 메서드
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
            if (recipe.items.Count == items.Count) // 먼저 리스트의 크기가 같은지 확인
            {
                bool allItemsMatch = true;
                for (int i = 0; i < items.Count; i++)
                {
                    if (recipe.items[i] != items[i]) // 각 위치의 아이템을 비교
                    {
                        allItemsMatch = false; // 하나라도 다르면 false
                        break;
                    }
                }
                if (allItemsMatch)
                {
                    return recipe; // 모든 아이템이 일치하면 해당 레시피를 반환
                }
            }
        }
        return null; // 매칭되는 레시피가 없다면 null 반환
    }
}
