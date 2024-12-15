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

    public GameObject healingPotionPrefab;
    public GameObject adrenalinePotionPrefab;
    public GameObject stealthPotionPrefab;

    private void Start()
    {
        createRecipes();
    }

    public void createRecipes()
    {
        recipes = new CraftingRecipe[3];

        // Recipe 1: Healing Potion
        CraftingRecipe healingPotion = new CraftingRecipe();
        healingPotion.requiredItems[0] = crimsonBloom;
        healingPotion.requiredItems[1] = crystal;
        healingPotion.requiredItems[2] = flask;
        healingPotion.resultPrefab = healingPotionPrefab;
        recipes[0] = healingPotion;

        // Recipe 2: Adrenaline Potion
        CraftingRecipe adrenalinePotion = new CraftingRecipe();
        adrenalinePotion.requiredItems[0] = flareBlossom;
        adrenalinePotion.requiredItems[1] = crystal;
        adrenalinePotion.requiredItems[2] = flask;
        adrenalinePotion.resultPrefab = adrenalinePotionPrefab;
        recipes[1] = adrenalinePotion;

        // Recipe 3: Stealth Potion
        CraftingRecipe stealthPotion = new CraftingRecipe();
        stealthPotion.requiredItems[0] = echoFlower;
        stealthPotion.requiredItems[1] = crystal;
        stealthPotion.requiredItems[2] = flask;
        stealthPotion.resultPrefab = stealthPotionPrefab;
        recipes[2] = stealthPotion;
    }

    public void CheckAndCraftItem()
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            if (RecipeMatches(recipe))
            {
                // Instantiate the result prefab
                GameObject newItemObject = Instantiate(recipe.resultPrefab);
                if (!newItemObject)
                {
                    Debug.LogError("Failed to instantiate the item prefab!");
                    return;
                }

                // Get the Item component
                Item craftedItem = newItemObject.GetComponent<Item>();
                if (craftedItem == null)
                {
                    Debug.LogError("No 'Item' component found on the prefab.");
                    Destroy(newItemObject);
                    return;
                }

                // Add the item to the craftedItem slot
                craftedItemSlot.AddItem(
                    craftedItem.itemName,
                    craftedItem.quantity,     // or 1 if you prefer
                    craftedItem.icon,
                    craftedItem.itemDescription,
                    craftedItem
                );

                // Clear the crafting slots
                foreach (var slot in craftingSlots)
                {
                    slot.ResetItemSlot();
                }

                Debug.Log($"Crafted: {craftedItem.itemName}");

                // If you don't need the prefab in the scene, destroy or deactivate it
                Destroy(newItemObject);
                // newItemObject.SetActive(false);

                return;
            }
        }

        Debug.Log("No matching recipe found.");
    }


    private bool RecipeMatches(CraftingRecipe recipe)
    {
        Debug.Log("Comparing recipe requirements...");

        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].item == null)
            {
                Debug.Log($"Crafting slot {i} is empty. Expected: {recipe.requiredItems[i]?.itemName}");
                return false;
            }

            // Compare item names rather than references
            if (craftingSlots[i].item.itemName != recipe.requiredItems[i].itemName)
            {
                Debug.Log($"Mismatch in crafting slot {i}. Found: {craftingSlots[i].item.itemName}, Expected: {recipe.requiredItems[i].itemName}");
                return false;
            }

            Debug.Log($"Crafting slot {i} matches: {craftingSlots[i].item.itemName}");
        }

        Debug.Log("All crafting slots match the recipe.");
        return true;
    }


}
