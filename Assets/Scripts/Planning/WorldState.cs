using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    public Vector3 cPos;
    public World.WorldStateMask mask;
    public List<Ingredient> ingredientsKept;

    public WorldState()
    {
        cPos = new Vector3();
        ingredientsKept = new List<Ingredient>();
    }

    public bool Compare(WorldState world)
    {
        return (cPos == world.cPos && mask == world.mask);
    }
}
