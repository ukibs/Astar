﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button [] ingredientsButtons;
    public Button [] recipesButtons;

    public Text currentRecipe;
    //public GameObject[] ingredientsPrefabs;

    public Text textRecipe;
    public Text textMoney;
    public Text textNumberRecipesDone;

    public GameObject player;

    private IngredientScript [] ingredients;
    public Recipe[] recipes;
    private int indexIngredient = 0;
    private int indexRecipes = 0;
    // Dinero del cocinero
    private int currentMoney;
    private int recipesDone = 0;
    private int indexRecipeDoing = -1;
    //
    private int currentPrefabToSet = -1;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = Resources.LoadAll<IngredientScript>("Scriptable/Ingredients");
        recipes = Resources.LoadAll<Recipe>("Scriptable/Recipes");
        indexIngredient = 0;
        currentMoney = 100;
        asignIngredients();
        asignRecipes();
    }

    void Update()
    {
        CheckMouse();
    }

    private void asignRecipes()
    {
        for (int i = 0; i < recipesButtons.Length; i++)
        {
            recipesButtons[i].enabled = true;
            recipesButtons[i].GetComponent<GUI_recipe>().index = indexRecipes + i;
            recipesButtons[i].GetComponent<GUI_recipe>().SetIngredient = recipes[indexRecipes + i];

            if(recipes[indexRecipes + i].cost > currentMoney)
            {
                recipesButtons[i].enabled = false;
            }
        }
    }

    private void asignIngredients()
    {
        for (int i = 0; i < ingredientsButtons.Length; i++)
        {
            ingredientsButtons[i].enabled = true;
            ingredientsButtons[i].GetComponent<GUI_Ingredient>().index = indexIngredient + i;
            ingredientsButtons[i].GetComponent<GUI_Ingredient>().SetIngredient = ingredients[indexIngredient+i];

            if(ingredients[indexIngredient + i].cost > currentMoney)
            {
                ingredientsButtons[i].enabled = false;
            }
        }
    }

    public void createPrefab(int index)
    {
        currentPrefabToSet = index;
        currentMoney -= ingredients[index].cost;
        textMoney.text = currentMoney + "";
        asignIngredients();
        asignRecipes();
    }

    private void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0) && currentPrefabToSet > -1)
        {
            PlacePrefab(Input.mousePosition);
        }
    }

    private void PlacePrefab(Vector3 mousePosition)
    {
        Vector3 posInWorld = Camera.main.ViewportToWorldPoint(mousePosition);
        //Raycast
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Hit?
        if (Physics.Raycast(ray, out hit, 1000.0f)) {
            //
            if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Unwalkable"))
            {
                posInWorld = hit.point;
                posInWorld.y = 1;
                //
                GameObject newIngredient = Instantiate(ingredients[currentPrefabToSet].prefab, posInWorld, Quaternion.identity);
                // Luego quiatermos dinero con el coste
                currentPrefabToSet = -1;
            }
            
        }
        
    }

    public void newRecipe(int index)
    {
        if (indexRecipeDoing == -1)
        {
            indexRecipeDoing = index;
            textRecipe.text = recipes[index].name;
            currentMoney -= recipes[index].cost;
            player.GetComponent<CookBehaviourTree>().Plannifier.fin.finalRecipe.Add(recipes[index]);
        }
    }

    public void AddCoin()
    {
        currentMoney++;
        textMoney.text = currentMoney + "";
    }

    public void ChangeIngredientIndex(int value)
    {
        int newIndex = indexIngredient + value;
        if(newIndex >= 0 && newIndex < ingredients.Length - 2)
        {
            indexIngredient = newIndex;
            asignIngredients();
        }
    }

    public void ChangeRecipeIndex(int value)
    {
        int newIndex = indexRecipes + value;
        if (newIndex >= 0 && newIndex < recipes.Length - 2)
        {
            indexRecipes = newIndex;
            asignRecipes();
        }
    }

    public void RecipeDone(Recipe recipe)
    {
        if (indexRecipeDoing != -1 && recipe.name == recipes[indexRecipeDoing].name)
        {
            recipesDone++;
            textNumberRecipesDone.text = recipesDone + "";
            currentMoney += recipes[indexRecipeDoing].profit;
            indexRecipeDoing = -1;
            textRecipe.text = "";
        }
    }
}
