using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Finders
{
    /// <summary>
    /// This class is responsible for keeping the methods and algorithms responsible for calculating and finding
    /// the most optimized path according to the A* pathfinding algorithm.
    /// </summary>
    public static class AStar
    {
        private static HashSet<Node> _closedSet;
        private static PriorityQueue<Node> _openSet;

        /// <summary>
        /// Finds the most optimized path based on the A* algorithm.
        /// </summary>
        public static List<Node> FindPath(Node start, Node goal)
        {
            // Initializing the lists
            _openSet = new PriorityQueue<Node>(Comparer<Node>.Create(((aNode, bNode) =>
                aNode.EstimatedCost < bNode.EstimatedCost ? -1 : aNode.EstimatedCost > bNode.EstimatedCost ? 1 : 0)));
            _closedSet = new HashSet<Node>();
        
            // Calculates the start node values and add to the open set
            start.TotalCost = 0f;
            start.EstimatedCost = Heuristic.Manhattan(start, goal);
            _openSet.Enqueue(start);
        
            Node currNode = null;
            while (_openSet.Length != 0)
            {
                currNode = _openSet.Peek();
                if (currNode != start && currNode != goal)
                {
                    currNode.IsFinished = true;
                }
            
                if (currNode == goal)
                {
                    return Util.ReconstructPath(currNode);
                }
                
                var neighbors = GridManager.Instance.GetNeighborsNodes(currNode);
                for (var i = 0; i < neighbors.Count; i++)
                {
                    var neighbor = neighbors[i];
                    if (!_closedSet.Contains(neighbor) && !neighbor.IsObstacle)
                    {
                        var neighborTotalCost = currNode.TotalCost + Heuristic.Manhattan(currNode, neighbor);
                        neighbor.Parent = currNode;
                        neighbor.TotalCost = neighborTotalCost;
                        neighbor.EstimatedCost = neighborTotalCost + Heuristic.Manhattan(neighbor, goal) + (neighbor.IsDiagonalNeighbor ? 1 : 2);
                        if (!_openSet.Contains(neighbor))
                        {
                            _openSet.Enqueue(neighbor);
                        }
                        if (neighbor != start && neighbor != goal)
                        {
                            neighbor.IsVisited = true;
                        }
                    }
                }
                
                _openSet.Remove(currNode);
                _closedSet.Add(currNode);
                
            }

            if (currNode == goal) return Util.ReconstructPath(currNode);
        
            Debug.LogError("It was not possible to find a path!");
            return null;
        }
    }
}