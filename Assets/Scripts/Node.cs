
using UnityEngine;

public class Node
{
    public Vector2Int position;
    public bool traversable;
    public float gCost; // Cost from start node
    public float hCost; // Heuristic cost to target node
    public Node parent;

    public float FCost => gCost + hCost;

    public Node(Vector2Int position, bool traversable)
    {
        this.position = position;
        this.traversable = traversable;
    }
}
