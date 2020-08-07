using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Provides utility functions to be used in the algorithms of the finders.
    /// </summary>
    public static class Util 
    {
        /// <summary>
        /// Reconstructs a path based on a current node and all of its parents.
        /// </summary>
        /// <param name="node">The current node to start the recursion to get the complete path traversing all of its parent nodes.</param>
        public static void ReconstructPath(Node node)
        {
            var path = new List<Node>();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            path.Reverse();
            DrawPath(path);
        }
        
        /// <summary>
        /// Draws the best path found by the algorithm.
        /// </summary>
        /// <param name="foundPath">A list containing all the nodes from the path to be drawn on the grid.</param>
        private static void DrawPath(List<Node> foundPath)
        {
            for (var i = 0; i < foundPath.Count; i++)
            {
                if (!foundPath[i].IsStartNode() && !foundPath[i].IsGoalNode())
                {
                    foundPath[i].IsVisited = false;
                    foundPath[i].IsFinished = false;
                    foundPath[i].IsBestPath = true;
                }
            }
        }
    }
    
}
