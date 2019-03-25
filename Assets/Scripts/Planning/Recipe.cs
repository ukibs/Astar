using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string name;
    public Ingredients[] ingredients = new Ingredients[3];
}
