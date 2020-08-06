using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class Util 
    {
        public static void ReconstructPath(Node node, List<Node> path)
        {
            // TODO: Remove this path from params. The draw method is already here.
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            path.Reverse();
        }
        
        public static void DrawPath(List<Node> foundPath)
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
