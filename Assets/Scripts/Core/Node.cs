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
        [SerializeField] private Material _nodeMaterial;
        [SerializeField] private Material _startMaterial;
        [SerializeField] private Material _goalMaterial;
        [SerializeField] private Material _pathMaterial;
        [SerializeField] private Material _obstacleMaterial;
        [SerializeField] private Material _visitedMaterial;
        [SerializeField] private Material _finishedMaterial;
        
        // Cached references.
        private MeshRenderer _meshRenderer;

        private bool _isObstacle;
        private bool _isVisited;
        private bool _isFinished;
        private bool _isBestPath;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (_isBestPath)
            {
                _meshRenderer.material = _pathMaterial;
                return;
            }

            if (_isObstacle)
            {
                _meshRenderer.material = _obstacleMaterial;
                return;
            }

            if (_isFinished)
            {
                _meshRenderer.material = _finishedMaterial;
                return;
            }

            if (_isVisited)
            {
                _meshRenderer.material = _visitedMaterial;
                return;
            }

            if (IsStartNode())
            {
                _meshRenderer.material = _startMaterial;
                return;
            }

            if (IsGoalNode())
            {
                _meshRenderer.material = _goalMaterial;
                return;
            }

            _meshRenderer.material = _nodeMaterial;
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

        public bool IsObstacle
        {
            get => _isObstacle;
            set => _isObstacle = value;
        }

        public bool IsBestPath
        {
            set => _isBestPath = value;
        }

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
