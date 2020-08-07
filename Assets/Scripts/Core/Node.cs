using System;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Represents a single Node in the grid.
    /// </summary>
    public class Node : MonoBehaviour, IComparable<Node>
    {

        [Header("// Node Materials")]
        [SerializeField] private Material _visitedMaterial;
        [SerializeField] private Material _finishedMaterial;

        // Cached references.
        private MeshRenderer _meshRenderer;

        private bool _isVisited;
        private bool _isFinished;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (_isFinished)
            {
                _meshRenderer.material = _finishedMaterial;
            } else if (_isVisited)
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

        public bool IsVisited
        {
            set
            {
                if (!IsStartNode() && !IsGoalNode())
                {
                    _isVisited = value;
                }
            }
        }

        public bool IsFinished
        {
            set
            {
                if (!IsStartNode() && !IsGoalNode())
                {
                    _isFinished = value;
                }
            }
        }

        public int CompareTo(Node other)
        {
            return EstimatedCost < other.EstimatedCost ? -1 : EstimatedCost > other.EstimatedCost ? 1 : 0;
        }
    }
}
