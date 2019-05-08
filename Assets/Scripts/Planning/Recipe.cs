using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe")]
public class Recipe: ScriptableObject
{
    public string name;
    public Ingredients[] ingredients = new Ingredients[3];
    [Range(5, 30)]
    public int cost = 5;
    [Range(5, 20)]
    public int profit = 5;
}
