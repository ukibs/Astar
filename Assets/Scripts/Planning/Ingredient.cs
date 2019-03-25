using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Ingredients
{
    Bread,
    Eggs,
    Count
}

public class Ingredient : MonoBehaviour
{
    public Ingredients type;
}

