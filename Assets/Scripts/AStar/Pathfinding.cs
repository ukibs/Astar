using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Heuristics
{
    Euclidean,
    Manhattan,
    Diagonal
}

public class Pathfinding : MonoBehaviour
{

    public Transform mSeeker;
    public Transform mTarget;

    AstarNode CurrentStartNode;
    AstarNode CurrentTargetNode;

    Grid Grid;

    int Iterations = 0;
    float LastStepTime = 0.0f;
    float TimeBetweenSteps = 0.01f;

    bool EightConnectivity = true;
    public Heuristics heuristic = Heuristics.Euclidean;

    #region Properties

    //public bool TransformHasTarget { get { return mTarget != null; } } 

    #endregion

    /***************************************************************************/

    void Awake()
    {
        Grid = FindObjectOfType<Grid>();

        Iterations = 0;
        LastStepTime = 0.0f;

        if (heuristic == Heuristics.Manhattan)
        {
            EightConnectivity = false;
        }
        else EightConnectivity = true;
    }

    /***************************************************************************/

    void Update()
    {
        // Positions changed?
        //if (PathInvalid())
        //{
        //    // Remove old path
        //    if (Grid.path != null)
        //    {
        //        Grid.path.Clear();
        //    }
        //    // Start calculating path again
        //    Iterations = 0;
        //    if (TimeBetweenSteps == 0.0f)
        //    {
        //        Iterations = -1;
        //    }
        //    FindPath(mSeeker.position, mTarget.position, Iterations);
        //}
        //else
        //{
        //    // Path found?
        //    if (Iterations >= 0)
        //    {
        //        // One or more iterations?
        //        if (TimeBetweenSteps == 0.0f)
        //        {
        //            // One iteration, look until path is found
        //            Iterations = -1;
        //            FindPath(mSeeker.position, mTarget.position, Iterations);
        //        }
        //        else if (Time.time > LastStepTime + TimeBetweenSteps)
        //        {
        //            // Iterate increasing depth every time step
        //            LastStepTime = Time.time;
        //            Iterations++;
        //            FindPath(mSeeker.position, mTarget.position, Iterations);
        //        }
        //    }
        //}
    }

    /***************************************************************************/

    bool PathInvalid()
    {
        return CurrentStartNode != Grid.NodeFromWorldPoint(mSeeker.position) ||
            CurrentTargetNode != Grid.NodeFromWorldPoint(mTarget.position);
    }

    /***************************************************************************/

    // List<Node> to worck with the smooth
    public List<AstarNode> FindPath(Vector3 startPos, Vector3 targetPos, int iterations)
    {
        CurrentStartNode = Grid.NodeFromWorldPoint(startPos);
        CurrentTargetNode = Grid.NodeFromWorldPoint(targetPos);

        List<AstarNode> openSet = new List<AstarNode>();
        HashSet<AstarNode> closedSet = new HashSet<AstarNode>();
        openSet.Add(CurrentStartNode);
        Grid.openSet = openSet;

        int currentIteration = 0;
        AstarNode node = CurrentStartNode;
        while (openSet.Count > 0 && node != CurrentTargetNode &&
              (iterations == -1 || currentIteration < iterations))
        {
            // Select first node from open list
            node = openSet[0];

            /****/
            //TODO
            /****/
            //Check if the first one is the best option
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost ||    //Get the smallest fCost
                    (openSet[i].fCost == node.fCost && openSet[i].gCost < node.gCost)) //If fCost is equal choose the one with less gCost
                {
                    node = openSet[i];
                }
            }

            // Manage open/closed list
            openSet.Remove(node);
            closedSet.Add(node);
            Grid.openSet = openSet;
            Grid.closedSet = closedSet;
            
            // Check destination
            if (node != CurrentTargetNode)
            {
                // Open neighbours
                foreach (AstarNode neighbour in Grid.GetNeighbours(node, EightConnectivity))
                {
                    /****/
                    //TODO
                    /****/
                    // First we check that the neighbor is walkable
                    if (!neighbour.mWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    // If the neighbor isn't already in the openset
                    // or the path to it is shorter than the current
                    float newGCost = node.gCost + GetDistance(node, neighbour) * neighbour.mCostMultiplier;
                    if (!openSet.Contains(neighbour) || newGCost < neighbour.gCost)
                    {
                        neighbour.hCost = Heuristic(neighbour, CurrentTargetNode);
                        neighbour.gCost = newGCost;
                        neighbour.mParent = node;
                        // Insert to evaluate it
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }

                currentIteration++;
            }
            else
            {
                // Path found!
                RetracePath(CurrentStartNode, CurrentTargetNode);

                // Path found
                Iterations = -1;

                //Debug.Log("Statistics:");
                //Debug.LogFormat("Total nodes:  {0}", openSet.Count + closedSet.Count);
                //Debug.LogFormat("Open nodes:   {0}", openSet.Count);
                //Debug.LogFormat("Closed nodes: {0}", closedSet.Count);
            }
        }

        return Grid.path;
    }

    /***************************************************************************/

    void RetracePath(AstarNode startNode, AstarNode endNode)
    {
        List<AstarNode> path = new List<AstarNode>();
        AstarNode currentNode = endNode;

        /****/
        //TODO
        /****/
        // Seguimos padres desde la meta
        while (currentNode != startNode)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.mParent;
        }
        
        path.Insert(0, startNode);

        Grid.path = path;
        //Grid.path = SmoothPath(path);
    }

    /***************************************************************************/

    float GetDistance(AstarNode nodeA, AstarNode nodeB)
    {
        // Distance function
        //nodeA.mGridX   nodeB.mGridX
        //nodeA.mGridY   nodeB.mGridY
        float distance = 0;

        switch (heuristic)
        {
            case Heuristics.Manhattan:
                distance = Mathf.Abs(nodeA.mGridX - nodeB.mGridX) + Mathf.Abs(nodeA.mGridY - nodeB.mGridY);
                break;
            case Heuristics.Euclidean:
                distance = Mathf.Sqrt(Mathf.Pow(nodeA.mGridX - nodeB.mGridX, 2) + Mathf.Pow(nodeA.mGridY - nodeB.mGridY, 2));
                break;
            case Heuristics.Diagonal:
                float dx = Mathf.Abs(nodeA.mGridX - nodeB.mGridX);
                float dy = Mathf.Abs(nodeA.mGridY - nodeB.mGridY);
                float min = Mathf.Min(dx, dy);
                float max = Mathf.Max(dx, dy);

                float diagonalSteps = min;
                float straightSteps = max - min;

                distance = Mathf.Sqrt(2) * diagonalSteps + straightSteps;
                break;
        }

        return distance;
    }

    /***************************************************************************/

    float Heuristic(AstarNode nodeA, AstarNode nodeB)
    {
        // Heuristic function
        //nodeA.mGridX   nodeB.mGridX
        //nodeA.mGridY   nodeB.mGridY

        /****/
        //TODO
        /****/
        
        return GetDistance(nodeA, nodeB);
    }

    /***************************************************************************/

    // SMOOTH & BRESENHAM --------------------------------------------------------------------------------------------

    /***************************************************************************/

    List<AstarNode> SmoothPath(List<AstarNode> path)
    {
        List<AstarNode> smoothPath = new List<AstarNode>();

        //Add the last node, the target one
        smoothPath.Add(path[path.Count-1]);

        //Set the first node to compare, the destiny
        AstarNode currentNode = smoothPath[0];
        
        int max = path.Count;
        bool aux = false;
        while(currentNode != path[0])
        {
            for (int j = 0; j < max; j++)
            {
                //Check inversely which node is necessary
                if (BresenhamWalkable(currentNode.mGridX, currentNode.mGridY, path[j].mGridX, path[j].mGridY))
                {
                    smoothPath.Insert(0, path[j]);
                    currentNode = path[j];  // Change the node comparing
                    max = j;
                    aux = true; //one node has been selected
                    break;
                }
            }
            //if a node haven't been selected means that the next one from the current one is necessary
            if (!aux)
            {
                smoothPath.Insert(0, path[max - 1]);
                currentNode = path[max - 1];
                max = max - 1;
            }
            aux = false;
        }
        smoothPath.Insert(0, path[0]);

        return smoothPath;
    }
    
    /***************************************************************************/

    /// <summary>
    /// Check if the distance between those points is short than the difference of the gCosts
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    public bool BresenhamWalkable(int x, int y, int x2, int y2)
    {
        //TODO: 4 Connectivity
        //TODO: Cost
        AstarNode destNode = Grid.GetNode(x2, y2);
        AstarNode startNode = Grid.GetNode(x, y);
        //
        float gCostDif = startNode.gCost - destNode.gCost;
        // Difference between x and y, to know how much we need to move
        int w = x2 - x;
        int h = y2 - y;

        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        // Move to the left
        if (w < 0)
            dx1 = -1;
        else if (w > 0) // Move to the right
            dx1 = 1;
        // 
        if (h < 0) //Move up
            dy1 = -1;
        else if (h > 0) //Move down
            dy1 = 1;
        
        //If you only have to move in x
        if (w < 0) //Left
            dx2 = -1;
        else if (w > 0) //Right
            dx2 = 1;

        // Width -> longest // height -> shortest
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);

        // if it is no longest change the values, and means there is no move on x, only on y
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            //Move in y
            if (h < 0) //Up
            {
                dy2 = -1;
            }
            else if (h > 0) //Down
            {
                dy2 = 1;
            }
            dx2 = 0; //No move in x
        }

        // Obetenmos numerator del valor de longest con sus bits desplazados una posición a la derecha
        // ex: 23 -> 11
        int numerator = longest >> 1;
        AstarNode cNode;
        float distance = 0.0f;

        // Ahora recorremos el camino entre los dos nodos 
        // Recorremos el camino a su largo
        for (int i = 0; i < longest; i++)
        {
            // Get the current node (x, y)
            cNode = Grid.GetNode(x, y);
            if (!cNode.mWalkable)
            {
                return false;   // No se puede atajar
            }

            // Vamos aumentando el numerator con el manhatan más corto
            // Nota: Si el camino es totalmente recto shortest debería ser 0
            numerator += shortest;
            // Y decidimos si avanzar en diagonal
            if (!(numerator < longest))
            {
                // Recorrido diagnoal (debería)
                numerator -= longest;
                x += dx1;
                y += dy1;

                distance += Mathf.Sqrt(2) * cNode.mCostMultiplier;

            }
            else
            {
                // Move in x or y only one will be different from 0
                x += dx2;
                y += dy2;

                distance += cNode.mCostMultiplier;
            }

            // check if distance is bigger than the cost of the node
            if(distance > gCostDif * 1.01f)     // * 1.01 to give a margin
            {
                Debug.Log("Can't tackle: distance " + distance + ", gCostDiff " + gCostDif);
                return false;
            }
        }

        Debug.Log("Can tackle: distance " + distance + ", gCostDiff " + gCostDif);
        return true;    //This nodes have a short connection  
    }

}
