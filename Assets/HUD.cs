using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button [] ingredientsButtons;
    public Button [] recipesButtons;

    public IngredientScript [] ingredients;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = Resources.LoadAll<IngredientScript>("Scriptable/Ingredients");
        index = 0;
        asignIngredients();
    }
    
    private void asignIngredients()
    {
        for (int i = 0; i < ingredientsButtons.Length; i++)
        {
            ingredientsButtons[i].GetComponent<GUI_Ingredient>().index = index + i;
            ingredientsButtons[i].GetComponent<GUI_Ingredient>().SetIngredient = ingredients[index+i];
        }
    }

    public void createPrefab(int index)
    {
        Debug.Log("He pinchado en " + index);
    }
}
