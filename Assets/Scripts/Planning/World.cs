using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class World : MonoBehaviour
{
    public List<NodePlanning> openSet;
    public HashSet<NodePlanning> closedSet;

    public List<NodePlanning> plan;

    List<Ingredient> ingredients = new List<Ingredient>();

    WorldState mWorldState;

    public List<PlanningAction> mActionList;

    public List<Recipe> recipes = new List<Recipe>();

    /***************************************************************************/

    public enum WorldStateMask
    {
        WORLD_STATE_NONE = 0,
        WS_RECIPE_DONE = 1,
        WS_CLOSE_TO_KITCHEN = 2
    }

    /***************************************************************************/

    void Awake()
    {
        mWorldState = new WorldState();
        mWorldState.cPos = transform.position;
        mActionList = new List<PlanningAction>();

        ingredients = FindObjectsOfType<Ingredient>().ToList();

        for (int i = 0; i < (int)Ingredients.Count; i++)
        {
            mActionList.Add(
              new PlanningAction(
                PlanningAction.ActionType.AT_GO_TO,
                (Ingredients)i,
                WorldStateMask.WORLD_STATE_NONE,
                WorldStateMask.WORLD_STATE_NONE,
                WorldStateMask.WORLD_STATE_NONE,
                10.0f, "Going to "+ (Ingredients)i)
            );
        }

        for (int i = 0; i < (int)Ingredients.Count; i++)
        {
            mActionList.Add(
              new PlanningAction(
                PlanningAction.ActionType.AT_PICK_UP,
                (Ingredients)i,
                WorldStateMask.WORLD_STATE_NONE,
                WorldStateMask.WORLD_STATE_NONE,
                WorldStateMask.WORLD_STATE_NONE,
                5.0f, "Picking up " + (Ingredients)i)
            );
        }

        mActionList.Add(
          new PlanningAction(
            PlanningAction.ActionType.AT_GO_TO_KITCHEN,
            Ingredients.Count,
            WorldStateMask.WORLD_STATE_NONE,
            WorldStateMask.WS_RECIPE_DONE,
            WorldStateMask.WORLD_STATE_NONE,
            10.0f, "In the kitchen with a delicious recipe")
        );

    }

    /***************************************************************************/

    #region Forward
    public List<NodePlanning> GetNeighbours(NodePlanning node)
    {
        List<NodePlanning> neighbours = new List<NodePlanning>();

        foreach (PlanningAction action in mActionList)
        {
            // If preconditions are met we can apply effects and the new state is valid
            if ((node.mWorldState.mask & action.mPreconditions) == action.mPreconditions  && MeetsAdditionalPreconditions( node.mWorldState, action ) )
            {
                // Apply action and effects
                NodePlanning newNodePlanning = new NodePlanning(node.mWorldState, action);
                newNodePlanning.mWorldState.mask |= action.mEffects;
                newNodePlanning.mWorldState.mask &= ~action.mNegEffects;
                ApplyAdditionalEffects(newNodePlanning, newNodePlanning.mWorldState, action);
                neighbours.Add(newNodePlanning);
            }
        }

        return neighbours;
    }

    public void ApplyAdditionalEffects(NodePlanning nodePlanning, WorldState mWorldState, PlanningAction action)
    {
        switch (action.mActionType)
        {
            case PlanningAction.ActionType.AT_GO_TO:
                Vector3 ingredientPos = FindIngredientOfType(action.mIngredient);
                nodePlanning.mAction.mCost = (ingredientPos - mWorldState.cPos).magnitude;
                mWorldState.cPos = ingredientPos;
                break;
            case PlanningAction.ActionType.AT_PICK_UP:
                mWorldState.ingredientsKept.Add(action.mIngredient);
                break;
            case PlanningAction.ActionType.AT_GO_TO_KITCHEN:
                mWorldState.finalRecipe.Add(RecipeCompleted(mWorldState));
                break;
            default:
                break;
        }
    }

    public bool MeetsAdditionalPreconditions(WorldState mWorldState, PlanningAction action)
    {
        bool meets = false;
        bool changePos = false;
        Vector3 ingredientPos = new Vector3();
        switch (action.mActionType)
        {
            case PlanningAction.ActionType.AT_GO_TO:
                meets = ValidIngredient(mWorldState, action.mIngredient);
                break;
            case PlanningAction.ActionType.AT_PICK_UP:
                ingredientPos = FindIngredientOfType(action.mIngredient);
                //Check if you already have the ingredient to not go again for it
                if (!mWorldState.ingredientsKept.Contains(action.mIngredient)) changePos = true;
                break;
            case PlanningAction.ActionType.AT_GO_TO_KITCHEN:
                meets = RecipeCompleted(mWorldState) != null ? true : false;
                break;
            default:
                meets = true;
                break;
        }
        if (changePos) meets = (ingredientPos - mWorldState.cPos).magnitude <= 2 ? true : false;
        return meets;
    }
    #endregion

    #region Backward
    public List<NodePlanning> GetNeighboursBackward(NodePlanning node)
    {
        List<NodePlanning> neighbours = new List<NodePlanning>();

        foreach (PlanningAction action in mActionList)
        {
            // If preconditions are met we can apply effects and the new state is valid
            if (MeetConditions(node.mWorldState, action))
            {
                // Apply action and effects
                NodePlanning newNodePlanning = new NodePlanning(node.mWorldState, action);
                ApplyBackwardEffects(newNodePlanning, newNodePlanning.mWorldState, action);
                neighbours.Add(newNodePlanning);
            }
        }

        return neighbours;
    }

    public void ApplyBackwardEffects(NodePlanning node, WorldState world, PlanningAction action)
    {
        switch(action.mActionType)
        {
            case PlanningAction.ActionType.AT_PICK_UP:
                world.ingredientsKept.Remove(action.mIngredient);
                
                break;
            case PlanningAction.ActionType.AT_GO_TO:
                Vector3 ingredientPos = FindIngredientOfType(action.mIngredient);
                node.mAction.mCost = (ingredientPos - mWorldState.cPos).magnitude;
                mWorldState.cPos = ingredientPos;
                world.ingredientsVisited.Remove(action.mIngredient);
                break;
        }
    }

    public bool MeetConditions(WorldState world, PlanningAction action)
    {
        bool meets = false;
        Vector3 ingredientPos = FindIngredientOfType(action.mIngredient);
        switch (action.mActionType)
        {
            case PlanningAction.ActionType.AT_GO_TO:
                meets = (!world.ingredientsKept.Contains(action.mIngredient) && world.finalRecipe[0].ingredients.Contains(action.mIngredient));
                break;
            case PlanningAction.ActionType.AT_PICK_UP:
                meets = (world.ingredientsKept.Contains(action.mIngredient) && (world.ingredientsVisited.Count) == world.ingredientsKept.Count);
                break;
        }

        return meets;
    }
    #endregion

    private bool ValidIngredient(WorldState world, Ingredients ingredient)
    {
        if (world.ingredientsKept.Count == 0) return true;

        bool valid = false;

        List<Ingredients> possibleIngredients = new List<Ingredients>();
        possibleIngredients = FindPossibleIngredients(world.ingredientsKept[0]);

        for (int i = 1; i < world.ingredientsKept.Count; i++)
        {
            List<Ingredients> ingredients = new List<Ingredients>();
            ingredients = FindPossibleIngredients(world.ingredientsKept[i]);
            for(int  j = 0; j < possibleIngredients.Count; j++)
            {
                if(!ingredients.Contains(possibleIngredients[j]))
                {
                    possibleIngredients.RemoveAt(j);
                }
            }
        }

        for(int i = 0; i < possibleIngredients.Count; i++)
        {
            if(possibleIngredients[i] == ingredient)
            {
                valid = true;
                break;
            }
        }

        return valid;
    }

    private List<Ingredients> FindPossibleIngredients(Ingredients ingredient)
    {
        List<Ingredients> list = new List<Ingredients>();

        for(int i = 0; i < recipes.Count; i++)
        {
            if(recipes[i].ingredients.Contains(ingredient))
            {
                for(int j = 0; j < recipes[i].ingredients.Length; j++)
                {
                    list.Add(recipes[i].ingredients[j]);
                }
            }
        }

        return list;
    }

    private Recipe RecipeCompleted(WorldState world)
    {
        foreach (Recipe r in recipes)
        {
            int equals = 0;
            foreach (Ingredients i in world.ingredientsKept)
            {
                foreach (Ingredients ing in r.ingredients)
                {
                    if (i == ing)
                    {
                        equals++;
                        break;
                    }
                }
            }
            if (equals == r.ingredients.Length)
            {
                return r;
            }
        }

        return null;
    }

    public Vector3 FindIngredientOfType(Ingredients type)
    {
        for(int i = 0; i < ingredients.Count; i++)
        {
            if(ingredients[i].type == type)
            {
                return ingredients[i].transform.position;
            }
        }
        return Vector3.one;
    }

    /***************************************************************************/

    public static int PopulationCount(int n)
    {
        return System.Convert.ToString(n, 2).ToCharArray().Count(c => c == '1');
    }

    /***************************************************************************/

}