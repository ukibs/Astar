using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Ingredient : MonoBehaviour
{

    private IngredientScript ingredient;
    public int index;

    public IngredientScript SetIngredient
    {
        set
        {
            ingredient = value;
            GetComponent<Image>().sprite = ingredient.sprite;
            //GetComponent<Button>().GetComponentInChildren<Text>().text = ingredient.name;
        }
    }

    public void click()
    {
        HUD hud = FindObjectOfType<HUD>();
        hud.createPrefab(index);
    }
}
