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
        WS_POTATO_OWNED = 2,
        WS_ONION_OWNED = 4,
        WS_EGGS_OWNED = 8,
        WS_MOJO_OWNED = 16,
        WS_SALT_OWNED = 32,
        WS_RICE_OWNED = 64,
        WS_CHICKEN_OWNED = 128,
        WS_BREAD_OWNED = 256,
        WS_CLOSE_TO_KITCHEN = 512
    }

    /***************************************************************************/

    void Awake()
    {
        mWorldState = new WorldState();
        mWorldState.cPos = transform.position;
        mActionList = new List<Action>();
        plan = new List<NodePlanning>();
     
        mActionList.Add(
            new Action(
            Action.ActionType.AT_GO_TO_BREAD,
            WorldStateMask.WORLD_STATE_NONE,
            WorldStateMask.WORLD_STATE_NONE,
            WorldStateMask.WORLD_STATE_NONE,
            100.0f, "Going to bread")
        );

        mActionList.Add(
           new Action(
           Action.ActionType.AT_PICK_UP_BREAD,
           WorldStateMask.WORLD_STATE_NONE,
           WorldStateMask.WS_BREAD_OWNED,
           WorldStateMask.WORLD_STATE_NONE,
           10.0f, "Picking up bread")
       );

        mActionList.Add(
            new Action(
            Action.ActionType.AT_GO_TO_EGGS,
            WorldStateMask.WORLD_STATE_NONE,
            WorldStateMask.WORLD_STATE_NONE,
            WorldStateMask.WORLD_STATE_NONE,
            10.0f, "Going to eggs")
        );

        mActionList.Add(
           new Action(
           Action.ActionType.AT_PICK_UP_EGGS,
           WorldStateMask.WORLD_STATE_NONE,
           WorldStateMask.WS_EGGS_OWNED,
           WorldStateMask.WORLD_STATE_NONE,
           10.0f, "Picking up eggs")
       );

        mActionList.Add(
          new Action(
            Action.ActionType.AT_GO_TO_KITCHEN,
            (WorldStateMask.WS_BREAD_OWNED | WorldStateMask.WS_EGGS_OWNED),
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
            case Action.ActionType.AT_GO_TO_BREAD:
                Vector3 ingredientPos = FindIngredientOfType(Ingredients.Bread);
                nodePlanning.mAction.mCost = (ingredientPos - mWorldState.cPos).magnitude;
                mWorldState.cPos = ingredientPos;
                break;
            case Action.ActionType.AT_GO_TO_EGGS:
                ingredientPos = FindIngredientOfType(Ingredients.Eggs);
                nodePlanning.mAction.mCost = (ingredientPos - mWorldState.cPos).magnitude;
                mWorldState.cPos = ingredientPos;
                break;
            case Action.ActionType.AT_PICK_UP_BREAD:
                mWorldState.ingredientsKept.Add(new Ingredient(Ingredients.Bread));
                break;
            case Action.ActionType.AT_PICK_UP_EGGS:
                mWorldState.ingredientsKept.Add(new Ingredient(Ingredients.Eggs));
                break;
            default:
                break;
        }
    }

    public bool MeetsAdditionalPreconditions(WorldState mWorldState, Action action)
    {
        bool meets = false;
        Vector3 ingredientPos = new Vector3();
        switch (action.mActionType)
        {
            case Action.ActionType.AT_PICK_UP_BREAD:
                ingredientPos = FindIngredientOfType(Ingredients.Bread);
                break;
            case Action.ActionType.AT_PICK_UP_EGGS:
                ingredientPos = FindIngredientOfType(Ingredients.Eggs);
                break;
            default:
                meets = true;
                break;
        }
        if(!meets) meets = (ingredientPos - mWorldState.cPos).magnitude <= 2 ? true : false;
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
        return new Vector3();
    }

    /***************************************************************************/

    public static int PopulationCount(int n)
    {
        return System.Convert.ToString(n, 2).ToCharArray().Count(c => c == '1');
    }

    /***************************************************************************/

}