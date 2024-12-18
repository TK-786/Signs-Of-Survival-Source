using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe
{
    public Item[] requiredItems = new Item[3];

    public GameObject resultPrefab;
}


public class CraftingManager : MonoBehaviour
{
    public ItemSlot[] craftingSlots;  
    public ItemSlot craftedItemSlot; 
    public CraftingRecipe[] recipes;  

    [Header("References to Item Prefabs or ScriptableObjects")]
    public Item crimsonBloom;
    public Item crystal;
    public Item flask;
    public Item flareBlossom;
    public Item echoFlower;
    public Item healingPotion1;
    public Item adrenalinePotion1;
    public Item stealthPotion1;

    public GameObject healingPotionPrefab;
    public GameObject adrenalinePotionPrefab;
    public GameObject stealthPotionPrefab;
    public GameObject UltimatePotionPrefab;

    private void Start()
    {
        createRecipes();
    }

    public void createRecipes()
    {
        recipes = new CraftingRecipe[4];

        CraftingRecipe healingPotion = new CraftingRecipe();
        healingPotion.requiredItems[0] = crimsonBloom;
        healingPotion.requiredItems[1] = crystal;
        healingPotion.requiredItems[2] = flask;
        healingPotion.resultPrefab = healingPotionPrefab;
        recipes[0] = healingPotion;

        CraftingRecipe adrenalinePotion = new CraftingRecipe();
        adrenalinePotion.requiredItems[0] = flareBlossom;
        adrenalinePotion.requiredItems[1] = crystal;
        adrenalinePotion.requiredItems[2] = flask;
        adrenalinePotion.resultPrefab = adrenalinePotionPrefab;
        recipes[1] = adrenalinePotion;

        CraftingRecipe stealthPotion = new CraftingRecipe();
        stealthPotion.requiredItems[0] = echoFlower;
        stealthPotion.requiredItems[1] = crystal;
        stealthPotion.requiredItems[2] = flask;
        stealthPotion.resultPrefab = stealthPotionPrefab;
        recipes[2] = stealthPotion;

        CraftingRecipe UltimatePotion = new CraftingRecipe();
        UltimatePotion.requiredItems[0] = healingPotion1;
        UltimatePotion.requiredItems[1] = adrenalinePotion1;
        UltimatePotion.requiredItems[2] = stealthPotion1;
        UltimatePotion.resultPrefab = UltimatePotionPrefab;
        recipes[3] = UltimatePotion;
    }

    public void PreviewCraftItem()
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            if (RecipeMatches(recipe))
            {
                GameObject newItemObject = Instantiate(recipe.resultPrefab);

                Item craftedItem = newItemObject.GetComponent<Item>();

                craftedItemSlot.AddItem(
                    craftedItem.ItemName,
                    craftedItem.Quantity,
                    craftedItem.Icon,
                    craftedItem.ItemDescription,
                    craftedItem
                );
                return;
            }
        }
        craftedItemSlot.ResetItemSlot();
    }

    private bool RecipeMatches(CraftingRecipe recipe)
    {
        if (craftingSlots.Any(slot => slot.item == null)) {  return false; }

        string[] craftSlotNames = craftingSlots
                                  .Select(s => s.item.ItemName)
                                  .OrderBy(n => n)
                                  .ToArray();

        string[] recipeNames = recipe.requiredItems
                                     .Select(r => r.ItemName)
                                     .OrderBy(n => n)
                                     .ToArray();

        return craftSlotNames.SequenceEqual(recipeNames);
    }



}
