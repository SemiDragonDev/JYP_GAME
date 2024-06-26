using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Crafting/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public List<Recipe> recipes;
}
