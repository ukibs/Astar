using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public enum Ingredients
{
    Bread,
    Eggs,
    Potato,
    Onion,
    Chicken,
    Rice,
    Salt,
    Mojo,
    Count
}

public class Ingredient : MonoBehaviour
{
    public Ingredients type;
}

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientScript : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public GameObject prefab;
}

