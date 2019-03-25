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

    public List<Ingredient> ingredients = new List<Ingredient>();

    public WorldState mWorldState;

    public List<Action> mActionList;

   

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
        mActionList = new List<Action>();

        for (int i = 0; i < (int)Ingredients.Count; i++)
        {
            mActionList.Add(
              new Action(
                Action.ActionType.AT_GO_TO,
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
              new Action(
                Action.ActionType.AT_PICK_UP,
                (Ingredients)i,
                WorldStateMask.WORLD_STATE_NONE,
                WorldStateMask.WORLD_STATE_NONE,
                WorldStateMask.WORLD_STATE_NONE,
                5.0f, "Picking up " + (Ingredients)i)
            );
        }

        mActionList.Add(
          new Action(
            Action.ActionType.AT_GO_TO_KITCHEN,
            Ingredients.Count,
            WorldStateMask.WORLD_STATE_NONE,
            WorldStateMask.WS_RECIPE_DONE,
            WorldStateMask.WORLD_STATE_NONE,
            10.0f, "In the kitchen with a delicious recipe")
        );

    }

    /***************************************************************************/

    public List<NodePlanning> GetNeighbours(NodePlanning node)
    {
        List<NodePlanning> neighbours = new List<NodePlanning>();

        foreach (Action action in mActionList)
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

    public void ApplyAdditionalEffects(NodePlanning nodePlanning, WorldState mWorldState, Action action)
    {
        switch (action.mActionType)
        {
            case Action.ActionType.AT_GO_TO:
                Vector3 ingredientPos = FindIngredientOfType(action.mIngredient);
                nodePlanning.mAction.mCost = (ingredientPos - mWorldState.cPos).magnitude;
                mWorldState.cPos = ingredientPos;
                break;
            case Action.ActionType.AT_PICK_UP:
                mWorldState.ingredientsKept.Add(action.mIngredient);
                break;
            default:
                break;
        }
    }

    public bool MeetsAdditionalPreconditions(WorldState mWorldState, Action action)
    {
        bool meets = false;
        bool changePos = false;
        Vector3 ingredientPos = new Vector3();
        switch (action.mActionType)
        {
            case Action.ActionType.AT_PICK_UP:
                ingredientPos = FindIngredientOfType(action.mIngredient);
                if (!mWorldState.ingredientsKept.Contains(action.mIngredient))  changePos = true;
                break;
            case Action.ActionType.AT_GO_TO_KITCHEN:
                meets = mWorldState.ingredientsKept.Count == 2 ? true: false;
                break;
            default:
                meets = true;
                break;
        }
        if(changePos) meets = (ingredientPos - mWorldState.cPos).magnitude <= 2 ? true : false;
        return meets;
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