using UnityEngine;
using System.Collections;

public class PlanningAction
{
  public ActionType         mActionType;
	public World.WorldStateMask   mPreconditions;
  public World.WorldStateMask mEffects;
    public World.WorldStateMask mNegEffects;
  public float              mCost;
  public string             mName;
    public Ingredients mIngredient;

  /***************************************************************************/

  public enum ActionType
  {
    ACTION_TYPE_NONE                      = -1,
    AT_GO_TO_KITCHEN                                ,
    AT_GO_TO,
    AT_PICK_UP,
        ACTION_TYPES
  }
	
  /***************************************************************************/

	public PlanningAction( ActionType actionType, Ingredients ingredient, World.WorldStateMask preconditions, World.WorldStateMask effects, 
        World.WorldStateMask negativeEffects, float cost, string name )
  {
    mActionType     = actionType;
    mIngredient = ingredient;
    mPreconditions  = preconditions;
    mEffects        = effects;
    mNegEffects     = negativeEffects;
    mCost           = cost;
    mName           = name;
  }
                                                      
  /***************************************************************************/

}
