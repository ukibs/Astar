using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class CookBehaviourTree : MonoBehaviour
{
    //
    //public GameObject cooker;

    //
    private Root behaviorTree;
    private Planning plannifier;
    private List<NodePlanning> plan;
    private int mCurrentAction = -1;
    private float mTimeStartAction = 0;
    //private List<string> mPlan;
    //
    private Unit cookerPathSeeker;
    private Pathfinding pathfinding;
    private World world;

    // Start is called before the first frame update
    void Start()
    {
        // Recogida de componenetes
        //if (cooker != null)
        //    cookerPathSeeker = cooker.GetComponent<Unit>();
        cookerPathSeeker = GetComponent<Unit>();
        pathfinding = GetComponent<Pathfinding>();
        world = FindObjectOfType<World>();

        //
        plannifier = GetComponent<Planning>();
        //plan = plannifier.Plan;
        Debug.Log("Getting plan");
        //Debug.Log(plan);
        //
        //behaviorTree = new Root(
        //    new Action(() => Debug.Log("Hello World!"))
        //);
        //
        behaviorTree = new Root(
            new Sequence(
                new Action((bool planning) =>
                {
                    // Hacer que genere uno diferente para cada uno
                    plan = plannifier.Plan;
                    return Action.Result.SUCCESS;
                })
                { Label = "Planning" },
                new Repeater(plan.Count,
                    new Sequence(
                        new Action((bool nextActionAvailable) =>
                        {
                            mCurrentAction++;
                            mTimeStartAction = Time.time;

                            if (mCurrentAction >= plan.Count)
                            {
                                return Action.Result.FAILED;
                            }
                            else
                            {
                                return Action.Result.SUCCESS;
                            }
                        }),
                        new Selector(
                            new Action((bool ingredientReached) =>
                            {
                            //
                            if (plan[mCurrentAction].mAction.mActionType == PlanningAction.ActionType.AT_GO_TO)
                                {
                                //
                                if (cookerPathSeeker.movingState == MovingState.Stopped)
                                    {
                                        cookerPathSeeker.GetPath(plan[mCurrentAction].mWorldState.cPos);
                                        return Action.Result.PROGRESS;
                                    }
                                // TODO: Hacer un buen cálculo de la distancia
                                else if (cookerPathSeeker.movingState == MovingState.InDestiny)
                                    {
                                    //
                                    //cooker.transform.position = plan[mCurrentAction].mWorldState.cPos;
                                    //
                                    Debug.Log("He llegado a " + plan[mCurrentAction].mAction.mIngredient);
                                        cookerPathSeeker.movingState = MovingState.Stopped;
                                        return Action.Result.SUCCESS;
                                    }
                                    else /*if(cookerPathSeeker.movingState == MovingState.Moving)*/
                                    {
                                        return Action.Result.PROGRESS;
                                    }
                                }

                                return Action.Result.FAILED;
                            }),
                            new Action((bool ingredientOwned) =>
                            {
                            //
                            if (plan[mCurrentAction].mAction.mActionType == PlanningAction.ActionType.AT_PICK_UP)
                                {

                                //
                                bool itemInInventory = true;
                                // TODO: Chequearlo bien
                                if (itemInInventory)
                                    {
                                    //
                                    GameObject ingredientToGet = world.GetIngredientOfType(plan[mCurrentAction].mAction.mIngredient);
                                    //
                                    if (ingredientToGet != null)
                                        {
                                        //
                                        Debug.Log(plan[mCurrentAction].mAction.mIngredient + " recogido");
                                            cookerPathSeeker.gatheredIngredients.Add(plan[mCurrentAction].mAction.mIngredient);
                                            Destroy(ingredientToGet);
                                            return Action.Result.SUCCESS;
                                        }
                                    //
                                    Debug.Log(plan[mCurrentAction].mAction.mIngredient + " no está!");
                                        return Action.Result.FAILED;
                                    }
                                    else
                                    {
                                        return Action.Result.PROGRESS;
                                    }
                                }
                                else
                                {
                                    return Action.Result.FAILED;
                                }

                            }),
                            new Action((bool kitchenReached) =>
                            {
                            //
                            if (plan[mCurrentAction].mAction.mActionType == PlanningAction.ActionType.AT_GO_TO_KITCHEN)
                                {
                                //
                                if (cookerPathSeeker.movingState == MovingState.Stopped)
                                    {
                                        cookerPathSeeker.GetPath(plan[mCurrentAction].mWorldState.cPos);
                                        return Action.Result.PROGRESS;
                                    }
                                // TODO: Hacer un buen cálculo de la distancia
                                else if (cookerPathSeeker.movingState == MovingState.InDestiny)
                                    {
                                    //
                                    //cooker.transform.position = plan[mCurrentAction].mWorldState.cPos;
                                    //
                                    Debug.Log("He llegado a la cocina");
                                        cookerPathSeeker.movingState = MovingState.Stopped;
                                        return Action.Result.SUCCESS;
                                    }
                                    else /*if(cookerPathSeeker.movingState == MovingState.Moving)*/
                                    {
                                        return Action.Result.PROGRESS;
                                    }
                                }

                                return Action.Result.FAILED;

                            })
                        )
                    )
                )
            )
        );
        
        // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behaviorTree;
#endif
        //
        behaviorTree.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
