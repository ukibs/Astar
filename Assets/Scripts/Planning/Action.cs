using UnityEngine;
using System.Collections;

public class Action
{
  public ActionType         mActionType;
	public World.WorldState   mPreconditions;
  public World.WorldState   mEffects;
    public World.WorldState mNegEffects;
  public float              mCost;
  public string             mName;

  /***************************************************************************/

  public enum ActionType
  {
    ACTION_TYPE_NONE                      = -1,
    AT_DO_RECIPE                                    ,
    AT_GO_TO_KITCHEN                                ,
    AT_GO_TO_POTATO                                 ,
    AT_GO_TO_ONION                                  ,
    AT_GO_TO_EGGS                                   ,
    AT_GO_TO_MOJO                                   ,
    AT_GO_TO_SALT                                   ,
    AT_GO_TO_RICE                                   ,
    AT_GO_TO_CHICKEN                                ,
    AT_GO_TO_BREAD                                  ,
    AT_PICK_UP_POTATO                               ,
    AT_PICK_UP_ONION                                ,
    AT_PICK_UP_EGGS                                 ,
    AT_PICK_UP_MOJO                                 ,
    AT_PICK_UP_SALT                                 ,
    AT_PICK_UP_RICE                                 ,
    AT_PICK_UP_CHICKEN                              ,
    AT_PICK_UP_BREAD                                ,
        //ACTION_TYPE_STAB                          ,
        //ACTION_TYPE_SHOOT                         ,
        //ACTION_TYPE_LOAD_GUN                      ,
        //ACTION_TYPE_PICK_UP_GUN                   ,
        //ACTION_TYPE_PICK_UP_KNIFE                 ,
        //ACTION_TYPE_GO_TO_ENEMY                   ,
        //ACTION_TYPE_GO_TO_GUN                     ,
        //ACTION_TYPE_GO_TO_KNIFE                   ,
        //ACTION_TYPE_GET_LINE_OF_SIGHT_TO_ENEMY    ,
        ACTION_TYPES
  }
	
  /***************************************************************************/

	public Action( ActionType actionType, World.WorldState preconditions, World.WorldState effects, 
        World.WorldState negativeEffects, float cost, string name )
  {
    mActionType     = actionType;
    mPreconditions  = preconditions;
    mEffects        = effects;
    mNegEffects     = negativeEffects;
    mCost           = cost;
    mName           = name;
  }
                                                      
  /***************************************************************************/

}
