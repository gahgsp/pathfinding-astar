using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for keeping the methods and algorithms responsible for calculating and finding
/// the most optimized path according to the A* pathfinding algorithm.
/// </summary>
public class AStar : MonoBehaviour
{
    private static HashSet<Node> _closedSet;
    private static PriorityQueue _openSet;

    /// <summary>
    /// Finds the most optimized path based on the A* algorithm.
    /// </summary>
    public static List<Node> FindPath(Node start, Node goal)
    {
        // Initializing the lists
        _openSet = new PriorityQueue();
        _closedSet = new HashSet<Node>();
        
        // Calculates the start node values and add to the open set
        start.TotalCost = 0f;
        start.EstimatedCost = HeuristicFunction(start, goal);
        _openSet.Add(start);
        
        Node currNode = null;
        while (_openSet.Length != 0)
        {
            currNode = _openSet.Peek();
            if (currNode == goal)
            {
                return ReconstructPath(currNode);
            }

            _openSet.Remove(currNode);
            _closedSet.Add(currNode);

            var neighbors = GridManager.Instance.GetNeighborsNodes(currNode);
            for (var i = 0; i < neighbors.Count; i++)
            {
                var neighbor = neighbors[i];
                if (!_closedSet.Contains(neighbor) && !neighbor.IsObstacle)
                {
                    var neighborTotalCost = currNode.TotalCost + HeuristicFunction(currNode, neighbor);
                    neighbor.Parent = currNode;
                    neighbor.TotalCost = neighborTotalCost;
                    neighbor.EstimatedCost = neighborTotalCost + HeuristicFunction(neighbor, goal) + (neighbor.IsDiagonalNeighbor ? 1 : 2);
                    if (!_openSet.Contains(neighbor))
                    {
                        _openSet.Add(neighbor);
                    }
                }
            }
        }

        if (currNode == goal) return ReconstructPath(currNode);
        
        Debug.LogError("It was not possible to find a path!");
        return null;
    }

    /// <summary>
    /// The heuristic function used to calculate the weight of each Node on the grid.
    /// At the moment, it is used the Manhattan Distance to calculate the weight of the Nodes.
    /// </summary>
    private static float HeuristicFunction(Node neighbor, Node goal)
    {
        var neighborPosition = neighbor.transform.position;
        var goalPosition = goal.transform.position;
        return Math.Abs(neighborPosition.x - goalPosition.x) + Math.Abs(neighborPosition.z - goalPosition.z);
    }

    /// <summary>
    /// Reconstructs the path from the starting point to the goal point.
    /// </summary>
    private static List<Node> ReconstructPath(Node node)
    {
        var path = new List<Node>();
        while (node != null)
        {
            path.Add(node);
            node = node.Parent;
        }

        path.Reverse();
        return path;
    }
}