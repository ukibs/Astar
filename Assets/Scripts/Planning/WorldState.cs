using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldState
{
    public Vector3 cPos;
    public World.WorldStateMask mask;
    public List<Ingredients> ingredientsKept;
    public List<Recipe> finalRecipe;

    public WorldState()
    {
        cPos = new Vector3();
        ingredientsKept = new List<Ingredients>();
        finalRecipe = new List<Recipe>();
    }

    public WorldState(WorldState w)
    {
        cPos = w.cPos;
        mask = w.mask;
        ingredientsKept = new List<Ingredients>();
        finalRecipe = new List<Recipe>();
        for (int i = 0; i < w.ingredientsKept.Count; i++)
        {
            ingredientsKept.Add(w.ingredientsKept[i]);
        }
    }

    public bool Compare(WorldState world)
    {
        int equals = 0;

        //Check that contains the same ingredients
        for(int i = 0; i <ingredientsKept.Count; i++)
        {
            for(int j = 0; j < world.ingredientsKept.Count; j++)
            {
                if(ingredientsKept[i] == world.ingredientsKept[j])
                {
                    equals++;
                    break;
                }
            }
        }

        return (cPos == world.cPos && mask == world.mask && equals == world.ingredientsKept.Count);
    }

    public bool CompareFinal(WorldState final)
    {
        int equals = 0;

        //Check that contains the same ingredients
        for (int i = 0; i < finalRecipe.Count; i++)
        {
            for (int j = 0; j < final.finalRecipe.Count; j++)
            {
                if (finalRecipe[i] == final.finalRecipe[j])
                {
                    equals++;
                    break;
                }
            }
        }

        return (equals == final.finalRecipe.Count);
    }
}
