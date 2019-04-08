using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class CookBehaviourTree : MonoBehaviour
{

    private Root behaviorTree;
    private Planning plannifier;
    private List<NodePlanning> plan;
    private int mCurrentAction = -1;
    private float mTimeStartAction = 0;
    //private List<string> mPlan;

    // Start is called before the first frame update
    void Start()
    {
        //
        plannifier = GetComponent<Planning>();
        plan = plannifier.Plan;
        Debug.Log("Getting plan");
        //Debug.Log(plan);
        //
        //behaviorTree = new Root(
        //    new Action(() => Debug.Log("Hello World!"))
        //);
        //

        //
        behaviorTree = new Root(
           new Sequence(
                new Action((bool planning) =>
                {
                    plan = plannifier.Plan;
                    return Action.Result.SUCCESS;
                }){ Label = "Planning"},
                new Repeater(plan.Count, 
                    new Sequence(
                        new Action( (bool nextActionAvailable) =>
                        {
                            mCurrentAction++;
                            mTimeStartAction = Time.time;

                            if(mCurrentAction >= plan.Count)
                            {
                                return Action.Result.FAILED;
                            }
                            else
                            {
                                return Action.Result.SUCCESS;
                            }
                        }),
                        new Selector(
                            new Action((bool ingredientReached)=>
                            {
                                //
                                if(plan[mCurrentAction].mAction.mActionType == PlanningAction.ActionType.AT_GO_TO)
                                {
                                    //
                                    Debug.Log("Voy a...");
                                    // Chequeamos la distancia
                                    float distance = 0.5f;
                                    // TODO: Hacer un buen cálculo de la distancia
                                    if (distance < 1)
                                    {
                                        return Action.Result.SUCCESS;
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
                            new Action((bool ingredientOwned)=>
                            {
                                //
                                if (plan[mCurrentAction].mAction.mActionType == PlanningAction.ActionType.AT_PICK_UP)
                                {
                                    //
                                    Debug.Log("Cogiendo...");
                                    //
                                    bool itemInInventory = true;
                                    // TODO: Chequearlo bien
                                    if (itemInInventory)
                                    {
                                        return Action.Result.SUCCESS;
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
                            new Action((bool kitchenReached)=>
                            {
                                //
                                if (plan[mCurrentAction].mAction.mActionType == PlanningAction.ActionType.AT_GO_TO_KITCHEN)
                                {
                                    //
                                    Debug.Log("Retorno a la cocina");
                                    // Chequeamos la distancia
                                    float distanceToKitchen = 0.5f;
                                    // TODO: Hacer un buen cálculo de la distancia
                                    if (distanceToKitchen < 1)
                                    {
                                        return Action.Result.SUCCESS;
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

    //
    //public void ReceivePlan(List<NodePlanning> newPlan)
    //{
    //    plan = newPlan;
    //    //
    //    behaviorTree = new Root(
    //        new Action(() => Debug.Log("Hello World!"))
    //    );
    //    behaviorTree.Start();
    //}
}
