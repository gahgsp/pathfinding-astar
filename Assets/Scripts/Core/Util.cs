using System.Collections.Generic;

namespace Core
{
    public static class Util 
    {
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
