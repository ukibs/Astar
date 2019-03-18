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

    public WorldState mWorldState;

    public List<Action> mActionList;

    /***************************************************************************/

    public enum WorldState
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
        WS_CLOSE_TO_KITCHEN = 512,
        WS_CLOSE_TO_POTATO = 1024,
        WS_CLOSE_TO_ONION = 2048,
        WS_CLOSE_TO_EGGS = 4096,
        WS_CLOSE_TO_MOJO = 8192,
        WS_CLOSE_TO_SALT = 16384,
        WS_CLOSE_TO_RICE = 32768,
        WS_CLOSE_TO_CHICKEN = 65536,
        WS_CLOSE_TO_BREAD = 131072
        /*WORLD_STATE_ENEMY_DEAD              =   1,
        WORLD_STATE_GUN_OWNED               =   2,
        WORLD_STATE_GUN_LOADED              =   4,
        WORLD_STATE_KNIFE_OWNED             =   8,
        WORLD_STATE_CLOSE_TO_ENEMY          =  16,
        WORLD_STATE_CLOSE_TO_GUN            =  32,
        WORLD_STATE_CLOSE_TO_KNIFE          =  64,
        WORLD_STATE_LINE_OF_SIGHT_TO_ENEMY  = 128
        //WORLD_STATE_NOT_CLOSE_TO_ENEMY      = */
    }

    /***************************************************************************/

    void Awake()
    {
        mActionList = new List<Action>();
        /*mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_STAB,
            WorldState.WORLD_STATE_CLOSE_TO_ENEMY | WorldState.WORLD_STATE_KNIFE_OWNED,
            WorldState.WORLD_STATE_ENEMY_DEAD,
            WorldState.WORLD_STATE_NONE,
            5.0f, "Stab" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_SHOOT,
            WorldState.WORLD_STATE_LINE_OF_SIGHT_TO_ENEMY | WorldState.WORLD_STATE_GUN_LOADED | WorldState.WORLD_STATE_GUN_OWNED,
            WorldState.WORLD_STATE_ENEMY_DEAD,
            WorldState.WORLD_STATE_NONE,
            100.0f, "Shoot" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_LOAD_GUN,
            WorldState.WORLD_STATE_GUN_OWNED,
            WorldState.WORLD_STATE_GUN_LOADED,
            WorldState.WORLD_STATE_NONE,
            1.0f, "Load gun" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_PICK_UP_GUN,
            WorldState.WORLD_STATE_CLOSE_TO_GUN,
            WorldState.WORLD_STATE_GUN_OWNED,
            WorldState.WORLD_STATE_NONE,
            1.0f, "Pick up gun" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_PICK_UP_KNIFE,
            WorldState.WORLD_STATE_CLOSE_TO_KNIFE,
            WorldState.WORLD_STATE_KNIFE_OWNED,
            WorldState.WORLD_STATE_NONE,
            1.0f, "Pick up knife" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_GO_TO_ENEMY,
            WorldState.WORLD_STATE_NONE,
            WorldState.WORLD_STATE_CLOSE_TO_ENEMY,
            WorldState.WORLD_STATE_CLOSE_TO_KNIFE | WorldState.WORLD_STATE_CLOSE_TO_GUN,
            1.0f, "Go to enemy" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_GO_TO_GUN,
            WorldState.WORLD_STATE_NONE,
            WorldState.WORLD_STATE_CLOSE_TO_GUN,
            WorldState.WORLD_STATE_CLOSE_TO_ENEMY | WorldState.WORLD_STATE_CLOSE_TO_KNIFE,
            20.0f, "Go to gun" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_GO_TO_KNIFE,
            WorldState.WORLD_STATE_NONE,
            WorldState.WORLD_STATE_CLOSE_TO_KNIFE,
            WorldState.WORLD_STATE_CLOSE_TO_ENEMY | WorldState.WORLD_STATE_CLOSE_TO_KNIFE,
            20.0f, "Go to knife" )
        );

        mActionList.Add(
          new Action( 
            Action.ActionType.ACTION_TYPE_GET_LINE_OF_SIGHT_TO_ENEMY,
            WorldState.WORLD_STATE_GUN_LOADED | WorldState.WORLD_STATE_GUN_OWNED,
            WorldState.WORLD_STATE_LINE_OF_SIGHT_TO_ENEMY,
            WorldState.WORLD_STATE_NONE,
            10.0f, "Get line of sight to enemy" )
        );*/

        mActionList.Add(
          new Action(
            Action.ActionType.AT_GO_TO_KITCHEN,
            (WorldState.WS_POTATO_OWNED | WorldState.WS_ONION_OWNED | WorldState.WS_EGGS_OWNED) |
            (WorldState.WS_POTATO_OWNED & WorldState.WS_MOJO_OWNED & WorldState.WS_SALT_OWNED) |
            (WorldState.WS_RICE_OWNED & WorldState.WS_CHICKEN_OWNED & WorldState.WS_SALT_OWNED) |
            (WorldState.WS_CHICKEN_OWNED & WorldState.WS_EGGS_OWNED & WorldState.WS_BREAD_OWNED),
            WorldState.WS_RECIPE_DONE,
            WorldState.WORLD_STATE_NONE,
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
            if ((node.mWorldState & action.mPreconditions) == action.mPreconditions  && MeetsAdditionalPreconditions( node.mWorldState, action ) )
            {
                // Apply action and effects
                NodePlanning newNodePlanning = new NodePlanning(node.mWorldState | action.mEffects & ~action.mNegEffects, action);
                neighbours.Add(newNodePlanning);
            }
        }

        return neighbours;
    }

    public bool MeetsAdditionalPreconditions(WorldState mWorldState, Action action)
    {
        return true;
    }

    /***************************************************************************/

    public static int PopulationCount(int n)
    {
        return System.Convert.ToString(n, 2).ToCharArray().Count(c => c == '1');
    }

    /***************************************************************************/

}