using System;
using System.Collections.Generic;
using Core;
using Finders;
using UnityEngine;

/// <summary>
/// This class is responsible for all the functions to manage and provide the grid of Nodes to execute the A* pathfinding algorithm.
/// </summary>
public class GridManager : MonoBehaviour
{
    [Header("// Grid Configuration")]
    [SerializeField] private int _length = 10; // X axis
    [SerializeField] private int _width = 10; // Z axis
    [SerializeField] private float _timeStep = 0.1f;
    
    [Header("// Node")]
    [SerializeField] private GameObject _node;
    
    [Header("// Path Materials")]
    [SerializeField] private Material _startMaterial;
    [SerializeField] private Material _goalMaterial;
    [SerializeField] private Material _pathMaterial;
    [SerializeField] private Material _obstacleMaterial;
    
    public Material PathMaterial => _pathMaterial;
    
    // Private properties.
    private GameObject[,] _nodes;
    private bool _isSelectingStartPoint = true;
    private bool _allowDiagonals = false;
    private int _algorithmOption = 0;

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

    private void Start()
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
        var startNode = GameObject.FindGameObjectWithTag("Start").GetComponent<Node>();
        var goalNode = GameObject.FindGameObjectWithTag("Goal").GetComponent<Node>();

        if (startNode == null || goalNode == null)
        {
            Debug.LogError("No Start/Goal node defined!");
            return;
        }
        
        switch (_algorithmOption)
        {
            case 0:
                StartCoroutine(AStar.FindPath(startNode, goalNode, _timeStep));
                break;
            case 1:
                StartCoroutine(Dijkstra.FindPath(_nodes, startNode, goalNode, _timeStep));
                break;
            default:
                StartCoroutine(BreadthFirstSearch.FindPath(startNode, goalNode, _timeStep));
                break;
        }
    }

    /// <summary>
    /// Event called when an algorithm is selected on the UI.
    /// </summary>
    /// <param name="option">The index of the algorithm option presented to the user.</param>
    public void OnSelectAlgorithm(int option)
    {
        _algorithmOption = option;
    }
    
    /// <summary>
    /// Returns all the neighbors nodes of the current node passed by parameter.
    /// </summary>
    /// <param name="node">The current node that will be used to find it's neighbors.</param>
    public List<Node> GetNeighborsNodes(Node node)
    {
        var nodeNeighbors = new List<Node>();
        var nodePosition = node.transform.position;
        var nodeRow = (int) (nodePosition.z);
        var nodeColumn = (int) (nodePosition.x);

        // Top
        if (nodeRow < _width - 1)
        {
            nodeNeighbors.Add(_nodes[nodeRow + 1, nodeColumn].GetComponent<Node>());
        }

        // Right
        if (nodeColumn < _length - 1)
        {
            nodeNeighbors.Add(_nodes[nodeRow, nodeColumn + 1].GetComponent<Node>());
        }
        
        // Bottom
        if (nodeRow > 0)
        {
            nodeNeighbors.Add(_nodes[nodeRow - 1, nodeColumn].GetComponent<Node>());
        }

        // Left
        if (nodeColumn > 0)
        {
            nodeNeighbors.Add(_nodes[nodeRow, nodeColumn - 1].GetComponent<Node>());
        }
        
        if (_allowDiagonals)
        {
            // Top Left Diagonal
            if (nodeRow < _width -1 && nodeColumn > 0)
            {
                var diagonalNode = _nodes[nodeRow + 1, nodeColumn - 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }
            
            // Top Right Diagonal
            if (nodeRow < _width - 1 && nodeColumn < _length - 1)
            {
                var diagonalNode = _nodes[nodeRow + 1, nodeColumn + 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }
            
            // Bottom Right Diagonal
            if (nodeRow > 0 && nodeColumn < _length - 1)
            {
                var diagonalNode = _nodes[nodeRow - 1, nodeColumn + 1].GetComponent<Node>();
                diagonalNode.IsDiagonalNeighbor = true;
                nodeNeighbors.Add(diagonalNode);
            }
            
            // Bottom Left Diagonal
            if (nodeRow > 0 && nodeColumn > 0)
            {
                var diagonalNode = _nodes[nodeRow - 1, nodeColumn - 1].GetComponent<Node>();
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
    /// Event called when the UI Checkbox is checked / unchecked.
    /// </summary>
    public void OnAllowDiagonals()
    {
        _allowDiagonals = !_allowDiagonals;
    }
    
    /// <summary>
    /// Generates a grid of Node entities based on the configurations provided for the size of the map.
    /// </summary>
    private void GenerateMap()
    {
        Array.Clear(_nodes, 0, _nodes.Length);
        for (var z = 0; z < _length; z++)
        {
            for (var x = 0; x < _width; x++)
            {
                var positionToSpawnNode =  new Vector3(x * 1f + (x * 0.05f), 0f, z * 1f + (z * 0.05f));
                var newNode = Instantiate(_node, positionToSpawnNode, Quaternion.identity, transform);
                newNode.name = "Node: [" + z + "," + x + "]";
                _nodes[z, x] = newNode;
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
}