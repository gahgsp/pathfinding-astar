using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for all the functions to manage and provide the grid of Nodes to execute the A* pathfinding algorithm.
/// </summary>
public class GridManager : MonoBehaviour
{
    [Header("// Grid Configuration")]
    [SerializeField] private int _length = 10; // X axis
    [SerializeField] private int _width = 10; // Z axis
    
    [Header("// Node")]
    [SerializeField] private GameObject _node;
    
    [Header("// Path Materials")]
    [SerializeField] private Material _startMaterial;
    [SerializeField] private Material _goalMaterial;
    [SerializeField] private Material _pathMaterial;
    [SerializeField] private Material _obstacleMaterial;

    // Private properties.
    private GameObject[,] _nodes;
    private bool _isSelectingStartPoint = true;
    private bool _allowDiagonals = false;

    #region Singleton
    private static GridManager _instance;
    public static GridManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GridManager>();
            }
            return _instance;
        }
    }
    #endregion
    
    void Start()
    {
        _nodes = new GameObject[_length, _width];
        GenerateMap();
    }

    private void Update()
    {
        OnNodeClick();
    }
    
    /// <summary>
    /// Run the A* pathfinding algorithm using the properties applied on the map by the user.
    /// </summary>
    public void FindPath()
    {
        var foundPath = AStar.FindPath(
            GameObject.FindGameObjectWithTag("Start").GetComponent<Node>(), 
            GameObject.FindGameObjectWithTag("Goal").GetComponent<Node>());
        if (foundPath != null)
        {
            DrawPath(foundPath);
        }
    }

    /// <summary>
    /// Draws the found path by the A* algorithm on the map.
    /// </summary>
    /// <param name="foundPath">The found path by the A* algorithm.</param>
    private void DrawPath(List<Node> foundPath)
    {
        foreach (var node in foundPath)
        {
            if (!node.IsStartNode() && !node.IsGoalNode())
            {
                node.GetComponent<MeshRenderer>().material = _pathMaterial;
            }
        }
    }

    /// <summary>
    /// Returns all the neighbors nodes of the current node passed by parameter.
    /// </summary>
    /// <param name="node">The current node that will be used to find it's neighbors.</param>
    public List<Node> GetNeighborsNodes(Node node)
    {
        var nodeNeighbors = new List<Node>();
        var nodePosition = node.transform.position;
        var nodeColumn = (int) (nodePosition.x);
        var nodeRow = (int) (nodePosition.z);
        
        if (nodeColumn < _length - 1) {
            nodeNeighbors.Add(_nodes[nodeColumn + 1, nodeRow].GetComponent<Node>());
        }
        if (nodeColumn > 0) {
            nodeNeighbors.Add(_nodes[nodeColumn - 1, nodeRow].GetComponent<Node>());
        }
        if (nodeRow < _width - 1) {
            nodeNeighbors.Add(_nodes[nodeColumn, nodeRow + 1].GetComponent<Node>());
        }
        if (nodeRow > 0) {
            nodeNeighbors.Add(_nodes[nodeColumn, nodeRow - 1].GetComponent<Node>());
        }

        if (_allowDiagonals)
        {
            if (nodeColumn > 0 && nodeRow > 0)
            {
                var diagonalNode = _nodes[nodeColumn - 1, nodeRow - 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }

            if (nodeColumn < _length - 1 && nodeRow > 0)
            {
                var diagonalNode = _nodes[nodeColumn + 1, nodeRow - 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }

            if (nodeColumn > 0 && nodeRow < _width - 1)
            {
                var diagonalNode = _nodes[nodeColumn - 1, nodeRow + 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }

            if (nodeColumn < _length - 1 && nodeRow < _width - 1)
            {
                var diagonalNode = _nodes[nodeColumn + 1, nodeRow + 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }
        }
        
        return nodeNeighbors;
    }

    /// <summary>
    /// Resets all the properties to it's default values.
    /// </summary>
    public void ResetPath()
    {
        ResetGrid();
        _isSelectingStartPoint = true;
    }
    
    /// <summary>
    /// Generates a grid of Node entities based on the configurations provided for the size of the map.
    /// </summary>
    private void GenerateMap()
    {
        Array.Clear(_nodes, 0, _nodes.Length);
        for (var x = 0; x < _length; x++)
        {
            for (var z = 0; z < _width; z++)
            {
                var positionToSpawnNode =  new Vector3(x * 1f + (x * 0.05f), 0f, z * 1f + (z * 0.05f));
                var newNode = Instantiate(_node, positionToSpawnNode, Quaternion.identity, transform);
                _nodes[x, z] = newNode;
            }
        }
    }
    
    /// <summary>
    /// Responsible to listen to click events on the mouse.
    /// </summary>
    private void OnNodeClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateStartPoint();
        } else if (Input.GetMouseButtonDown(1))
        {
            CreateObstacleNode();
        }
    }

    /// <summary>
    /// Marks a clicked Node as an obstacle on the map.
    /// </summary>
    private void CreateObstacleNode()
    {
        var clickedNode = RetrieveClickedNode();
        if (clickedNode != null)
        {
            clickedNode.GetComponent<Node>().DefineNodeAsObstacle();
            clickedNode.GetComponent<MeshRenderer>().material = _obstacleMaterial;
        }
    }

    /// <summary>
    /// Creates the starting and goal points when the user clicks on a Node.
    /// </summary>
    private void CreateStartPoint()
    {
        var clickedNode = RetrieveClickedNode();
        if (clickedNode != null)
        {
            clickedNode.GetComponent<MeshRenderer>().material = _isSelectingStartPoint ? _startMaterial : _goalMaterial;
            clickedNode.tag = _isSelectingStartPoint ? "Start" : "Goal";
            _isSelectingStartPoint = false;
        }
    }

    /// <summary>
    /// Checks if the click was on a Node and returns it. If not, returns null.
    /// </summary>
    private Node RetrieveClickedNode()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit) ? hit.collider.gameObject.GetComponent<Node>() : null;
    }
    
    /// <summary>
    /// Destroys the current map and generates a new one.
    /// </summary>
    private void ResetGrid()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        GenerateMap();
    }

    public void OnAllowDiagonals()
    {
        _allowDiagonals = !_allowDiagonals;
    }
}