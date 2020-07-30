using System.Collections.Generic;

/// <summary>
/// Data structure similar to a Queue or Stack, that has a "priority value" associated to each element.
/// Thus, making an element be served first based on the highest priority.
/// </summary>
public class PriorityQueue
{

    /// <summary>
    /// Inner list to keep track of the elements on the queue.
    /// </summary>
    private List<Node> _nodes = new List<Node>();

    /// <summary>
    /// Adds a new element to the queue.
    /// </summary>
    public void Add(Node node)
    {
        _nodes.Add(node);
        _nodes.Sort();
    }

    /// <summary>
    /// Removes an element from the queue and immediately sorts the queue to keep the priority-based order updated.
    /// </summary>
    public void Remove(Node node)
    {
        _nodes.Remove(node);
        _nodes.Sort();
    }

    /// <summary>
    /// Returns the first element of the queue or null when the list is empty.
    /// </summary>
    public Node Peek()
    {
        return _nodes.Count > 0 ? _nodes[0] : null;
    }

    /// <summary>
    /// Checks if an element is already on the queue.
    /// </summary>
    public bool Contains(Node node)
    {
        return _nodes.Contains(node);
    }

    /// <summary>
    /// Clear the queue removing all of its elements.
    /// </summary>
    public void Clear()
    {
        _nodes.Clear();
    }

    /// <summary>
    /// Returns the current length of the queue.
    /// </summary>
    public int Length => _nodes.Count;
}
