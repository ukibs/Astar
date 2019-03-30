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
    public List<Ingredients> ingredientsVisited;

    public WorldState()
    {
        cPos = new Vector3();
        ingredientsKept = new List<Ingredients>();
        finalRecipe = new List<Recipe>();
        ingredientsVisited = new List<Ingredients>();
    }

    public WorldState(WorldState w)
    {
        cPos = w.cPos;
        mask = w.mask;
        ingredientsKept = new List<Ingredients>();
        finalRecipe = new List<Recipe>();
        ingredientsVisited = new List<Ingredients>();

        for(int i = 0; i < w.ingredientsVisited.Count; i++)
        {
            ingredientsVisited.Add(w.ingredientsVisited[i]);
        }

        for (int i = 0; i < w.ingredientsKept.Count; i++)
        {
            ingredientsKept.Add(w.ingredientsKept[i]);
        }

        for(int i = 0; i < w.finalRecipe.Count; i++)
        {
            finalRecipe.Add(w.finalRecipe[i]);
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

    public bool CompareBackward(WorldState world)
    {
        int equals = 0;

        //Check that contains the same ingredients
        for (int i = 0; i < ingredientsKept.Count; i++)
        {
            for (int j = 0; j < world.ingredientsKept.Count; j++)
            {
                if (ingredientsKept[i] == world.ingredientsKept[j])
                {
                    equals++;
                    break;
                }
            }
        }

        int equalVisited = 0;

        for (int i = 0; i < ingredientsVisited.Count; i++)
        {
            for (int j = 0; j < world.ingredientsVisited.Count; j++)
            {
                if (ingredientsVisited[i] == world.ingredientsVisited[j])
                {
                    equalVisited++;
                    break;
                }
            }
        }

        return (cPos == world.cPos && equals == ingredientsKept.Count && equalVisited == ingredientsVisited.Count);
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

    public bool CompareFinalBackwards(WorldState final)
    {

        return (ingredientsKept.Count == final.finalRecipe.Count && ingredientsVisited.Count == final.ingredientsVisited.Count);
    }
}
