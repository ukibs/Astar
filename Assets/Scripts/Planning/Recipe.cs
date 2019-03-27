using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe")]
public class Recipe: ScriptableObject
{
    public string name;
    public Ingredients[] ingredients = new Ingredients[3];
}
