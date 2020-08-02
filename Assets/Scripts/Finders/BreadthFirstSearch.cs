using System.Collections.Generic;
using Core;

namespace Finders
{
    public static class BreadthFirstSearch
    {

        private static HashSet<Node> _discoveredSet;
        private static Queue<Node> _openSet;
        
        public static List<Node> FindPath(Node start, Node goal)
        {
            _openSet = new Queue<Node>();
            _discoveredSet = new HashSet<Node> {start};

            _openSet.Enqueue(start);

            while (_openSet.Count > 0)
            {
                var currentNode = _openSet.Dequeue();
                if (currentNode != start && currentNode != goal)
                {
                    currentNode.IsFinished = true;
                }

                if (currentNode == goal)
                {
                    return Util.ReconstructPath(currentNode);
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
            }
            
            return null;
        }
    }
}
