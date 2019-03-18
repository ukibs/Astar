﻿using UnityEngine;
using System.Collections;

public class Action
{
  public ActionType         mActionType;
	public World.WorldStateMask   mPreconditions;
  public World.WorldStateMask mEffects;
    public World.WorldStateMask mNegEffects;
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
        ACTION_TYPES
  }
	
  /***************************************************************************/

	public Action( ActionType actionType, World.WorldStateMask preconditions, World.WorldStateMask effects, 
        World.WorldStateMask negativeEffects, float cost, string name )
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
