using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Planning : MonoBehaviour
{
    NodePlanning CurrentStartNode;
    NodePlanning CurrentTargetNode;

    public WorldState finBackward;
    public WorldState fin;

    private World mWorld;
    //private CookBehaviourTree behaviourTree;

    public List<NodePlanning> Plan { get { return mWorld.plan; } }

    /***************************************************************************/

    void Start()
    {
        //mWorld = GetComponent<World>();
        mWorld = FindObjectOfType<World>();
        //behaviourTree = GetComponent<CookBehaviourTree>();
        // Para que no se raye si esta vacio
        if (finBackward.finalRecipe != null)
        {
            for (int i = 0; i < finBackward.finalRecipe[0].ingredients.Length; i++)
            {
                finBackward.ingredientsKept.Add(finBackward.finalRecipe[0].ingredients[i]);
                finBackward.ingredientsVisited.Add(finBackward.finalRecipe[0].ingredients[i]);
            }
        }        

        Debug.Log("Planning...");
        FindPlan(new WorldState(), fin);

        //behaviourTree.ReceivePlan(mWorld.plan);
        //mWorld.plan = null;

        //Debug.Log("Planning backwards...");
        //FindPlanBackward(finBackward, new WorldState());
    }

    /***************************************************************************/

    void Update()
    {
    }

    /***************************************************************************/

    public List<NodePlanning> FindPlan(WorldState startWorldState, WorldState targetWorldState)
    {
        CurrentStartNode = new NodePlanning(startWorldState, null);
        CurrentTargetNode = new NodePlanning(targetWorldState, null);

        List<NodePlanning> openSet = new List<NodePlanning>();
        HashSet<NodePlanning> closedSet = new HashSet<NodePlanning>();
        openSet.Add(CurrentStartNode);
        mWorld.openSet = openSet;

        NodePlanning node = CurrentStartNode;
        while (openSet.Count > 0 && !node.mWorldState.CompareFinal(CurrentTargetNode.mWorldState))
        {
            // Select best node from open list
            node = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || (openSet[i].fCost == node.fCost && openSet[i].hCost < node.hCost))
                {
                    node = openSet[i];
                }
            }

            // Manage open/closed list
            openSet.Remove(node);
            closedSet.Add(node);
            mWorld.openSet = openSet;
            mWorld.closedSet = closedSet;



            // Check destination
            if (!node.mWorldState.CompareFinal(CurrentTargetNode.mWorldState))
            {

                // Open neighbours
                foreach (NodePlanning neighbour in mWorld.GetNeighbours(node))
                {
                    if ( /*!neighbour.mWalkable ||*/ closedSet.Any(n => n.mWorldState.Compare(neighbour.mWorldState)))
                    {
                        continue;
                    }

                    float newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Any(n => n.mWorldState.Compare(neighbour.mWorldState)))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = Heuristic(neighbour, CurrentTargetNode);
                        neighbour.mParent = node;

                        if (!openSet.Any(n => n.mWorldState.Compare(neighbour.mWorldState)))
                        {
                            openSet.Add(neighbour);
                            mWorld.openSet = openSet;
                        }
                        else
                        {
                            // Find neighbour and replace
                            openSet[openSet.FindIndex(x => x.mWorldState.Compare(neighbour.mWorldState))] = neighbour;
                        }
                    }
                }
            }
            else
            {
                // Path found!

                // End node must be copied
                CurrentTargetNode.mParent = node.mParent;
                CurrentTargetNode.mAction = node.mAction;
                CurrentTargetNode.gCost = node.gCost;
                CurrentTargetNode.hCost = node.hCost;

                RetracePlan(CurrentStartNode, CurrentTargetNode);

                Debug.Log("Statistics:");
                Debug.LogFormat("Total nodes:  {0}", openSet.Count + closedSet.Count);
                Debug.LogFormat("Open nodes:   {0}", openSet.Count);
                Debug.LogFormat("Closed nodes: {0}", closedSet.Count);
            }
        }

        // Log plan
        if (mWorld.plan != null)
            Debug.Log("PLAN FOUND!");
        else Debug.Log("Not plan found");
        for (int i = 0; i < mWorld.plan.Count; ++i)
        {
            Debug.LogFormat("{0} Accumulated cost: {1}", mWorld.plan[i].mAction.mName, mWorld.plan[i].gCost);
        }

        return mWorld.plan;
    }

    public List<NodePlanning> FindPlanBackward(WorldState startWorldState, WorldState targetWorldState)
    {
        CurrentStartNode = new NodePlanning(startWorldState, null);
        CurrentTargetNode = new NodePlanning(targetWorldState, null);

        List<NodePlanning> openSet = new List<NodePlanning>();
        HashSet<NodePlanning> closedSet = new HashSet<NodePlanning>();
        openSet.Add(CurrentStartNode);
        mWorld.openSet = openSet;

        NodePlanning node = CurrentStartNode;
        while (openSet.Count > 0 && !node.mWorldState.CompareFinalBackwards(CurrentTargetNode.mWorldState))
        {
            // Select best node from open list
            node = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || (openSet[i].fCost == node.fCost && openSet[i].hCost < node.hCost))
                {
                    node = openSet[i];
                }
            }

            // Manage open/closed list
            openSet.Remove(node);
            closedSet.Add(node);
            mWorld.openSet = openSet;
            mWorld.closedSet = closedSet;



            // Check destination
            if (!node.mWorldState.CompareFinalBackwards(CurrentTargetNode.mWorldState))
            {

                // Open neighbours
                foreach (NodePlanning neighbour in mWorld.GetNeighboursBackward(node))
                {
                    if ( /*!neighbour.mWalkable ||*/ closedSet.Any(n => n.mWorldState.CompareBackward(neighbour.mWorldState)))
                    {
                        continue;
                    }

                    float newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Any(n => n.mWorldState.CompareBackward(neighbour.mWorldState)))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = Heuristic(neighbour, CurrentTargetNode);
                        neighbour.mParent = node;

                        if (!openSet.Any(n => n.mWorldState.CompareBackward(neighbour.mWorldState)))
                        {
                            openSet.Add(neighbour);
                            mWorld.openSet = openSet;
                        }
                        else
                        {
                            // Find neighbour and replace
                            openSet[openSet.FindIndex(x => x.mWorldState.CompareBackward(neighbour.mWorldState))] = neighbour;
                        }
                    }
                }
            }
            else
            {
                // Path found!

                // End node must be copied
                CurrentTargetNode.mParent = node.mParent;
                CurrentTargetNode.mAction = node.mAction;
                CurrentTargetNode.gCost = node.gCost;
                CurrentTargetNode.hCost = node.hCost;

                RetracePlan(CurrentStartNode, CurrentTargetNode);

                Debug.Log("Statistics:");
                Debug.LogFormat("Total nodes:  {0}", openSet.Count + closedSet.Count);
                Debug.LogFormat("Open nodes:   {0}", openSet.Count);
                Debug.LogFormat("Closed nodes: {0}", closedSet.Count);
            }
        }

        // Log plan
        if (mWorld.plan != null)
            Debug.Log("PLAN FOUND!");
        else Debug.Log("Not plan found");
        for (int i = 0; i < mWorld.plan.Count; ++i)
        {
            Debug.LogFormat("{0} Accumulated cost: {1}", mWorld.plan[i].mAction.mName, mWorld.plan[i].gCost);
        }

        return mWorld.plan;
    }

    /***************************************************************************/

    void RetracePlan(NodePlanning startNode, NodePlanning endNode)
    {
        List<NodePlanning> plan = new List<NodePlanning>();

        NodePlanning currentNode = endNode;

        while (currentNode != startNode)
        {
            plan.Add(currentNode);
            currentNode = currentNode.mParent;
        }
        plan.Reverse();

        mWorld.plan = plan;
    }

    /***************************************************************************/

    float GetDistance(NodePlanning nodeA, NodePlanning nodeB)
    {
        // Distance function
        return nodeB.mAction.mCost;
    }

    /***************************************************************************/

    float Heuristic(NodePlanning nodeA, NodePlanning nodeB)
    {
        // Heuristic function
        //return -World.PopulationCount( (int)(nodeA.mWorldState | nodeB.mWorldState) ) - World.PopulationCount( (int)(nodeA.mWorldState & nodeB.mWorldState) );
        return 0;
    }

    /***************************************************************************/

}
