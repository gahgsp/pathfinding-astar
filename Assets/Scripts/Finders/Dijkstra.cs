using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Finders
{
   /// <summary>
   /// This class is responsible for keeping the methods and algorithms responsible for calculating and finding
   /// the most optimized path according to the Dijkstra algorithm.
   /// Based upon: https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
   /// </summary>
   public static class Dijkstra
   {
      private static PriorityQueue<Node> _unexploredSet;

      public static IEnumerator FindPath(GameObject[,] nodes, Node start, Node goal, float timeStep)
      {
         _unexploredSet = new PriorityQueue<Node>(DijkstraComparer());

         for (var cols = 0; cols < 10; cols++)
         {
            for (var rows = 0; rows < 10; rows++)
            {
               var currentNodeScript = nodes[cols, rows].GetComponent<Node>();
               currentNodeScript.TotalCost = currentNodeScript == start ? 0f : float.PositiveInfinity;
               _unexploredSet.Enqueue(currentNodeScript);
            }
         }

         Node currentNode = null;
         while (_unexploredSet.Length > 0)
         {
            currentNode = _unexploredSet.Peek();
            
            if (currentNode == goal)
            {
               Util.ReconstructPath(currentNode);
               yield break;
            }
            
            currentNode.IsFinished = true;

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
                     neighbor.IsVisited = true;
                  }
               } 
            }
            
            yield return new WaitForSeconds(timeStep);
            
            _unexploredSet.Remove(currentNode);
            
         }
      
         if (currentNode == goal)
         {
            Util.ReconstructPath(currentNode);
            yield break;
         }
        
         Debug.LogError("It was not possible to find a path!");
         yield return null;
      }

      private static Comparer<Node> DijkstraComparer()
      {
         return Comparer<Node>.Create((aNode, bNode) =>
            aNode.TotalCost < bNode.TotalCost ? -1 : aNode.TotalCost > bNode.TotalCost ? 1 : 0);
      }
   }
}
