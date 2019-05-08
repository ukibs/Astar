using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button [] ingredientsButtons;
    public Button [] recipesButtons;
    public Text currentRecipe;
    public GameObject[] ingredientsPrefabs;

    private IngredientScript [] ingredients;
    private Recipe[] recipes;
    private int indexIngredient = 0;
    private int indexRecipes = 0;
    // Dinero del cocinero
    private int currentMoney;
    //
    private int currentPrefabToSet = -1;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = Resources.LoadAll<IngredientScript>("Scriptable/Ingredients");
        recipes = Resources.LoadAll<Recipe>("Scriptable/Recipes");
        indexIngredient = 0;
        asignIngredients();
        asignRecipes();
    }

    void Update()
    {
        
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
        currentPrefabToSet = index;
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
        posInWorld.y = 1;
        GameObject newIngredient = Instantiate(ingredientsPrefabs[currentPrefabToSet], posInWorld, Quaternion.identity);
        // Luego quiatermos dinero con el coste
        currentPrefabToSet = -1;
    }

    public void newRecipe(int index)
    {
        currentRecipe.text = recipes[index].name;
    }

    public void AddCoin()
    {
        currentMoney++;
        Debug.Log("Current money: " + currentMoney);
    }
}
