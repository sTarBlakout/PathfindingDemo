
using UnityEngine;

public class Node
{
    public Vector2Int Position;
    public bool Traversable;
    public float GCost; // Cost from start node
    public float HCost; // Heuristic cost to target node
    public Node Parent;

    public float FCost => GCost + HCost;

    public Node(Vector2Int position, bool traversable)
    {
        Position = position;
        Traversable = traversable;
    }
}
