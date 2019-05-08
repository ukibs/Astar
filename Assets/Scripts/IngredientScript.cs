using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientScript : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public GameObject prefab;
}
