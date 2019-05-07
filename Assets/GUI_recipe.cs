using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_recipe : MonoBehaviour
{
    private Recipe recipe;
    public int index;

    public Recipe SetIngredient
    {
        set
        {
            recipe = value;
            //GetComponent<Image>().sprite = recipe.sprite;
            GetComponent<Button>().GetComponentInChildren<Text>().text = recipe.name;
        }
    }

    public void click()
    {
        HUD hud = FindObjectOfType<HUD>();
        hud.newRecipe(index);
    }
}
