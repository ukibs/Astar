using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button [] ingredientsButtons;
    public Button [] recipesButtons;
    public Text textRecipe;
    public Text textMoney;

    private IngredientScript [] ingredients;
    private Recipe[] recipes;
    private int indexIngredient = 0;
    private int indexRecipes = 0;
    // Dinero del cocinero
    private int currentMoney;       

    // Start is called before the first frame update
    void Start()
    {
        ingredients = Resources.LoadAll<IngredientScript>("Scriptable/Ingredients");
        recipes = Resources.LoadAll<Recipe>("Scriptable/Recipes");
        indexIngredient = 0;
        asignIngredients();
        asignRecipes();
    }
    
    private void asignRecipes()
    {
        for (int i = 0; i < recipesButtons.Length; i++)
        {
            recipesButtons[i].GetComponent<GUI_recipe>().index = indexIngredient + i;
            recipesButtons[i].GetComponent<GUI_recipe>().SetIngredient = recipes[indexRecipes + i];
        }
    }

    private void asignIngredients()
    {
        for (int i = 0; i < ingredientsButtons.Length; i++)
        {
            ingredientsButtons[i].GetComponent<GUI_Ingredient>().index = indexIngredient + i;
            ingredientsButtons[i].GetComponent<GUI_Ingredient>().SetIngredient = ingredients[indexIngredient+i];
        }
    }

    public void createPrefab(int index)
    {
        Debug.Log("He pinchado en " + index);
    }

    public void newRecipe(int index)
    {
        textRecipe.text = recipes[index].name;
    }

    public void AddCoin()
    {
        currentMoney++;
        textMoney.text = currentMoney + "";
    }

    public void ChangeIngredientIndex(int value)
    {
        int newIndex = indexIngredient + value;
        if(newIndex >= 0 || newIndex < ingredients.Length - 4)
        {
            indexIngredient = newIndex;
            asignIngredients();
        }
        Debug.Log(indexIngredient);
    }
}
