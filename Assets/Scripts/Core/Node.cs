using System;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Represents a single Node in the grid.
    /// </summary>
    public class Node : MonoBehaviour, IComparable<Node>
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
        
        public bool IsStartNode()
        {
            return gameObject.CompareTag("Start");
        }

        public bool IsGoalNode()
        {
            return gameObject.CompareTag("Goal");
        }

        public float TotalCost { get; set; } // G

        public float EstimatedCost { get; set; } // F

        public Node Parent { get; set; }

        public bool IsObstacle { get; private set; }

        public bool IsDiagonalNeighbor { get; set; }
    
        public bool IsVisited { get; set; }
    
        public bool IsFinished { get; set; }
        public int CompareTo(Node other)
        {
            return EstimatedCost < other.EstimatedCost ? -1 : EstimatedCost > other.EstimatedCost ? 1 : 0;
        }
    }
}
