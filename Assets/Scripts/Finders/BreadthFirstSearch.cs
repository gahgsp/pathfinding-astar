﻿using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Finders
{
    /// <summary>
    /// This class is responsible for keeping the methods and algorithms responsible for calculating and finding
    /// the most optimized path according to the Breadth First Search algorithm.
    /// Based upon: https://en.wikipedia.org/wiki/Breadth-first_search
    /// </summary>
    public static class BreadthFirstSearch
    {
        private static HashSet<Node> _discoveredSet;
        private static Queue<Node> _openSet;
        
        public static IEnumerator FindPath(Node start, Node goal, float timeStep)
        {
            _openSet = new Queue<Node>();
            _discoveredSet = new HashSet<Node> {start};

            _openSet.Enqueue(start);

            Node currentNode = null;
            while (_openSet.Count > 0)
            {
                currentNode = _openSet.Dequeue();
                currentNode.IsFinished = true;
                
                if (currentNode == goal)
                {
                    Util.ReconstructPath(currentNode);
                    yield break;
                }

                var neighbors = GridManager.Instance.GetNeighborsNodes(currentNode);
                for (var i = 0; i < neighbors.Count; i++)
                {
                    var neighbor = neighbors[i];
                    if (!_discoveredSet.Contains(neighbor) && !neighbor.IsObstacle)
                    {
                        _discoveredSet.Add(neighbor);
                        neighbor.Parent = currentNode;
                        _openSet.Enqueue(neighbor);
                        neighbor.IsVisited = true;
                    }
                }
                
                yield return new WaitForSeconds(timeStep);
                
            }
            
            if (currentNode == goal)
            {
                Util.ReconstructPath(currentNode);
            }
        
            Debug.LogError("It was not possible to find a path!");
            yield return null;
        }
    }
}
