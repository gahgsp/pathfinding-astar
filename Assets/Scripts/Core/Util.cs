using System.Collections.Generic;
using UnityEngine;

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
            foreach (var node in foundPath)
            {
                if (!node.IsStartNode() && !node.IsGoalNode())
                {
                    node.IsVisited = false;
                    node.IsFinished = false;
                    node.GetComponent<MeshRenderer>().material = GridManager.Instance.PathMaterial;
                }
            }
        }
    }
    
}
