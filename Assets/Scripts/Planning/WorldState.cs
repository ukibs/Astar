using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    public Vector3 cPos;
    public World.WorldStateMask mask;
    public List<Ingredients> ingredientsKept;

    public WorldState()
    {
        cPos = new Vector3();
        ingredientsKept = new List<Ingredients>();
    }

    public WorldState(WorldState w)
    {
        cPos = w.cPos;
        mask = w.mask;
        ingredientsKept = new List<Ingredients>();
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
}
