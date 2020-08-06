using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Finders
{
    public static class BreadthFirstSearch
    {

        private static HashSet<Node> _discoveredSet;
        private static Queue<Node> _openSet;
        private static List<Node> _foundPath;
        
        public static IEnumerator FindPath(Node start, Node goal, float timeStep)
        {
            _foundPath = new List<Node>();
            
            _openSet = new Queue<Node>();
            _discoveredSet = new HashSet<Node> {start};

            _openSet.Enqueue(start);

            Node currentNode = null;
            while (_openSet.Count > 0)
            {
                currentNode = _openSet.Dequeue();
                if (currentNode != start && currentNode != goal)
                {
                    currentNode.IsFinished = true;
                }

                if (currentNode == goal)
                {
                    Util.ReconstructPath(currentNode, _foundPath);
                    Util.DrawPath(_foundPath);
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
                        if (neighbor != start && neighbor != goal)
                        {
                            neighbor.IsVisited = true;
                        }
                    }
                }
                
                yield return new WaitForSeconds(timeStep);
                
            }
            
            if (currentNode == goal)
            {
                Util.ReconstructPath(currentNode, _foundPath);
                Util.DrawPath(_foundPath);
            }
        
            Debug.LogError("It was not possible to find a path!");
            yield return null;
        }
    }
}
