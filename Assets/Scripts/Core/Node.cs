using System;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Represents a single Node in the grid.
    /// </summary>
    public class Node : MonoBehaviour, IComparable
    {

        [SerializeField] private Material _visitedMaterial;
        [SerializeField] private Material _finishedMaterial;

        private MeshRenderer _meshRenderer;
        
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (IsFinished)
            {
                _meshRenderer.material = _finishedMaterial;
            } else if (IsVisited)
            {
                _meshRenderer.material = _visitedMaterial;
            }
        }

        /// <summary>
        /// Marks the Node as an obstacle, so this Node isn't accountable for the pathfinding algorithm.
        /// </summary>
        public void DefineNodeAsObstacle()
        {
            IsObstacle = true;
        }
    
        public int CompareTo(object obj)
        {
            var node = (Node) obj;
            return EstimatedCost < node.EstimatedCost ? -1 : EstimatedCost > node.EstimatedCost ? 1 : 0;
        }

        public bool IsStartNode()
        {
            return gameObject.CompareTag("Start");
        }

        public bool IsGoalNode()
        {
            return gameObject.CompareTag("Goal");
        }
    
        public float TotalCost { get; set; }

        public float EstimatedCost { get; set; }

        public Node Parent { get; set; }

        public bool IsObstacle { get; private set; }

        public bool IsDiagonalNeighbor { get; set; }
    
        public bool IsVisited { get; set; }
    
        public bool IsFinished { get; set; }
    }
}
