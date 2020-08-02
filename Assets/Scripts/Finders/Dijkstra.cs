using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Finders
{
   public static class Dijkstra
   {
      private static PriorityQueue<Node> _unexploredSet;

      public static List<Node> FindPath(GameObject[,] nodes, Node start, Node goal)
      {
         _unexploredSet = new PriorityQueue<Node>(Comparer<Node>.Create(((aNode, bNode) =>
            aNode.TotalCost < bNode.TotalCost ? -1 : aNode.TotalCost > bNode.TotalCost ? 1 : 0)));

         for (var cols = 0; cols < 10; cols++)
         {
            for (var rows = 0; rows < 10; rows++)
            {
               var currentNodeScript = nodes[cols, rows].GetComponent<Node>();
               currentNodeScript.TotalCost = currentNodeScript == start ? 0f : float.PositiveInfinity;
               _unexploredSet.Enqueue(currentNodeScript);
            }
         }

         while (_unexploredSet.Length > 0)
         {
            var currentNode = _unexploredSet.Peek();
            
            if (currentNode == goal)
            {
               return Util.ReconstructPath(currentNode);
            }
            
            if (currentNode != start && currentNode != goal)
            {
               currentNode.IsFinished = true;
            }
            
            var neighbors = GridManager.Instance.GetNeighborsNodes(currentNode);
            for (var i = 0; i < neighbors.Count; i++)
            {
               var neighbor = neighbors[i];
               if (!neighbor.IsObstacle && _unexploredSet.Contains(neighbor))
               {
                  var potentialCost = currentNode.TotalCost + Heuristic.Manhattan(currentNode, neighbor);
                  if (potentialCost < neighbor.TotalCost)
                  {
                     neighbor.TotalCost = potentialCost;
                     neighbor.Parent = currentNode;
                     if (neighbor != start && neighbor != goal)
                     {
                        neighbor.IsVisited = true;
                     }
                  }
               } 
            }
            
            _unexploredSet.Remove(currentNode);
            
         }
      
         return null;
      }
   }
}
