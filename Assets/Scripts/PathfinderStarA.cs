
using System.Collections.Generic;
using UnityEngine;

public class PathfinderStarA 
{
    public List<Vector2Int> FindPath(Node[,] nodes, Vector2Int start, Vector2Int target, ManagerUI.Heuristic heuristic)
    {
        var path = new List<Vector2Int>();

        var openNodes = new List<Node>();
        var closedNodes = new HashSet<Node>();
        
        var startNode = nodes[start.x, start.y];
        var targetNode = nodes[target.x, target.y];
        
        openNodes.Add(startNode);
        
        while (openNodes.Count > 0)
        {
            var currentNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].FCost < currentNode.FCost || 
                    (openNodes[i].FCost == currentNode.FCost && openNodes[i].HCost < currentNode.HCost))
                {
                    currentNode = openNodes[i];
                }
            }
            
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            
            if (currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                return path;
            }
            
            foreach (Node neighbor in GetNeighbors(nodes, currentNode))
            {
                if (!neighbor.Traversable || closedNodes.Contains(neighbor))
                    continue;
                
                float tentativeGCost = currentNode.GCost + 1;
                
                if (!openNodes.Contains(neighbor) || tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = CalculateHCost(neighbor.Position, target, heuristic);
                    neighbor.Parent = currentNode;
                    
                    if (!openNodes.Contains(neighbor))
                        openNodes.Add(neighbor);
                }
            }
        }
        
        return path;
    }
    
    private float CalculateHCost(Vector2Int from, Vector2Int to, ManagerUI.Heuristic heuristic)
    {
        if (heuristic == ManagerUI.Heuristic.Manhattan)
        {
            return Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y);
        }
        else // Chebyshev
        {
            int dx = Mathf.Abs(to.x - from.x);
            int dy = Mathf.Abs(to.y - from.y);
            return Mathf.Max(dx, dy);
        }
    }
    
    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Vector2Int>();
        var currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse(); 
        return path;
    }
    
    private List<Node> GetNeighbors(Node[,] nodes, Node node)
    {
        var neighbors = new List<Node>();

        int width = nodes.GetLength(0);
        int height = nodes.GetLength(1);
        
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = node.Position + dir;

            if (neighborPos.x >= 0 && neighborPos.x < width && neighborPos.y >= 0 && neighborPos.y < height)
            {
                neighbors.Add(nodes[neighborPos.x, neighborPos.y]);
            }
        }

        return neighbors;
    }
}
