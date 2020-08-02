using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Finders
{
   public static class Dijkstra
   {
      private static PriorityQueue _unexploredSet;

      public static List<Node> FindPath(GameObject[,] nodes, Node start, Node goal)
      {
         _unexploredSet = new PriorityQueue();

         for (var cols = 0; cols < 10; cols++)
         {
            for (var rows = 0; rows < 10; rows++)
            {
               var currentNode = nodes[cols, rows];
               var currentNodeScript = currentNode.GetComponent<Node>();
               if (currentNodeScript == start)
               {
                  currentNodeScript.EstimatedCost = 0f;
               }
               else
               {
                  currentNodeScript.EstimatedCost = float.PositiveInfinity;
               }
               _unexploredSet.Add(currentNodeScript);
            }
         }

         while (_unexploredSet.Length > 0)
         {
            var currentNode = _unexploredSet.Peek();
            if (currentNode != start && currentNode != goal)
            {
               currentNode.IsFinished = true;
            }
         
            _unexploredSet.Remove(currentNode);
         
            if (currentNode == goal)
            {
               return Util.ReconstructPath(currentNode);
            }

            var neighbors = GridManager.Instance.GetNeighborsNodes(currentNode);
            for (var i = 0; i < neighbors.Count; i++)
            {
               var neighbor = neighbors[i];
               if (!neighbor.IsObstacle && _unexploredSet.Contains(neighbor))
               {
                  var potentialCost = currentNode.EstimatedCost + Heuristic.Manhattan(currentNode, neighbor);
                  if (potentialCost < neighbor.EstimatedCost)
                  {
                     neighbor.EstimatedCost = potentialCost;
                     neighbor.Parent = currentNode;
                     if (neighbor != start && neighbor != goal)
                     {
                        neighbor.IsVisited = true;
                     }
                  }
               } 
            }
         }
      
         return null;
      }
   }
}
