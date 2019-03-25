using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Ingredients
{
    Bread,
    Eggs,
    Potato
}

public class Ingredient : MonoBehaviour
{
    public Ingredients type;

    public Ingredient(Ingredients t)
    {
        type = t;
    }
}

