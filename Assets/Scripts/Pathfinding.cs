using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{

    public Transform mSeeker;
    public Transform mTarget;

    Node CurrentStartNode;
    Node CurrentTargetNode;

    Grid Grid;

    int Iterations = 0;
    float LastStepTime = 0.0f;
    float TimeBetweenSteps = 0.01f;

    bool EightConnectivity = true;


    /***************************************************************************/

    void Awake()
    {
        Grid = GetComponent<Grid>();

        Iterations = 0;
        LastStepTime = 0.0f;
    }

    /***************************************************************************/

    void Update()
    {
        // Positions changed?
        if (PathInvalid())
        {
            // Remove old path
            if (Grid.path != null)
            {
                Grid.path.Clear();
            }
            // Start calculating path again
            Iterations = 0;
            if (TimeBetweenSteps == 0.0f)
            {
                Iterations = -1;
            }
            FindPath(mSeeker.position, mTarget.position, Iterations);
        }
        else
        {
            // Path found?
            if (Iterations >= 0)
            {
                // One or more iterations?
                if (TimeBetweenSteps == 0.0f)
                {
                    // One iteration, look until path is found
                    Iterations = -1;
                    FindPath(mSeeker.position, mTarget.position, Iterations);
                }
                else if (Time.time > LastStepTime + TimeBetweenSteps)
                {
                    // Iterate increasing depth every time step
                    LastStepTime = Time.time;
                    Iterations++;
                    FindPath(mSeeker.position, mTarget.position, Iterations);
                }
            }
        }
    }

    /***************************************************************************/

    bool PathInvalid()
    {
        return CurrentStartNode != Grid.NodeFromWorldPoint(mSeeker.position) ||
            CurrentTargetNode != Grid.NodeFromWorldPoint(mTarget.position);
    }

    /***************************************************************************/

    // List<Node> to worck with the smoothing stuff
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos, int iterations)
    {
        CurrentStartNode = Grid.NodeFromWorldPoint(startPos);
        CurrentTargetNode = Grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(CurrentStartNode);
        Grid.openSet = openSet;

        int currentIteration = 0;
        Node node = CurrentStartNode;
        while (openSet.Count > 0 && node != CurrentTargetNode &&
              (iterations == -1 || currentIteration < iterations))
        {
            // Select best node from open list
            node = openSet[0];

            /****/
            //TODO
            /****/

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || 
                    (openSet[i].fCost == node.fCost && openSet[i].gCost < node.gCost))
                {
                    // Elegimos aqui el siguiente
                    node = openSet[i];
                }
            }

            // Manage open/closed list
            openSet.Remove(node);
            // Insertar ordenado en vez de con ADD???
            closedSet.Add(node);
            Grid.openSet = openSet;
            Grid.closedSet = closedSet;


            // Check destination
            if (node != CurrentTargetNode)
            {
                // Open neighbours
                foreach (Node neighbour in Grid.GetNeighbours(node, EightConnectivity))
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

                Debug.Log("Statistics:");
                Debug.LogFormat("Total nodes:  {0}", openSet.Count + closedSet.Count);
                Debug.LogFormat("Open nodes:   {0}", openSet.Count);
                Debug.LogFormat("Closed nodes: {0}", closedSet.Count);

                //return closedSet;
            }
        }

        return Grid.path;
    }

    /***************************************************************************/

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

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

        Grid.path = SmoothPath(path);
    }

    /***************************************************************************/

    float GetDistance(Node nodeA, Node nodeB)
    {
        // Distance function
        //nodeA.mGridX   nodeB.mGridX
        //nodeA.mGridY   nodeB.mGridY
        if (EightConnectivity)
        {
            /****/
            //TODO
            /****/
            float distance = Mathf.Sqrt(Mathf.Pow(nodeA.mGridX - nodeB.mGridX, 2) + Mathf.Pow(nodeA.mGridY - nodeB.mGridY, 2));
            return distance;
        }
        else
        {
            /****/
            //TODO
            /****/
            float distance = Mathf.Abs(nodeA.mGridX - nodeB.mGridX) + Mathf.Abs(nodeA.mGridY - nodeB.mGridY);
            return distance;
        }
    }

    /***************************************************************************/

    float Heuristic(Node nodeA, Node nodeB)
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

    // BRESENHAM STUFF --------------------------------------------------------------------------------------------

    /***************************************************************************/

    List<Node> SmoothPath(List<Node> path)
    {
        //TODO
        List<Node> smoothPath = new List<Node>();

        smoothPath.Add(path[path.Count-1]);

        Node currentNode = smoothPath[0];
        
        int max = path.Count;
        bool aux = false;
        while(currentNode != path[0])
        {
            for (int j = 0; j < max; j++)
            {
                if (BresenhamWalkable(currentNode.mGridX, currentNode.mGridY, path[j].mGridX, path[j].mGridY))
                {
                    smoothPath.Insert(0, path[j]);
                    currentNode = path[j];
                    max = j;
                    Debug.Log(max);
                    aux = true;
                    break;
                }
            }
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

    public bool BresenhamWalkable(int x, int y, int x2, int y2)
    {
        //TODO: 4 Connectivity
        //TODO: Cost
        Node destNode = Grid.GetNode(x2, y2);
        Node startNode = Grid.GetNode(x, y);
        // Primero distancia manhatan entre xy y x2y2
        int w = x2 - x;
        int h = y2 - y;

        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        // 
        if (w < 0)
            dx1 = -1;
        else if (w > 0)
            dx1 = 1;
        // 
        if (h < 0)
            dy1 = -1;
        else if (h > 0)
            dy1 = 1;
        //
        if (w < 0)
            dx2 = -1;
        else if (w > 0)
            dx2 = 1;
        // Ponemos el ancho en longest
        // y el alto en shortest
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        // Y si no es mas largo, que lo sea
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            // Y aquí asigna el dy2
            if (h < 0)
            {
                dy2 = -1;
            }
            else if (h > 0)
            {
                dy2 = 1;
            }
            dx2 = 0;
        }
        // Parece que da longest o 1 si es más corto
        int numerator = longest >> 1;
        Node cNode;
        float distance = 0.0f;
        // Ahora recorremos el camino entre los dos nodos 
        // Siguiendo una línea recta o diagonal
        // Según los dx1, dy1 y dx2, dy2 que hayamos sacado
        for (int i = 0; i <= longest; i++)
        {
            cNode = Grid.GetNode(x, y);
            if (!cNode.mWalkable)
            {
                return false;
            }

            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;

                distance += cNode.mCostMultiplier;

            }
            else
            {
                x += dx2;
                y += dy2;

                distance += Mathf.Sqrt(2) * cNode.mCostMultiplier;

            }
        }
        
        float gCostDif = startNode.gCost - destNode.gCost;
        if (gCostDif * 1.5f >= distance )
        {
            return true;
        }
        else return false;        
    }

}
