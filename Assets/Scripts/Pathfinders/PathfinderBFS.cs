using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinders
{
    public class PathfinderBFS : IPathfinder //Greedy version of BFS
    {
        public List<Vector2Int> FindPath(Node[,] nodes, Vector2Int start, Vector2Int target, ManagerUI.Heuristic heuristic)
        {
            var closedSet = new List<Node>();
            var currentNode = nodes[start.x, start.y];

            while (currentNode.Position != target)
            {
                closedSet.Add(currentNode);
                Node nextNode = ExploreNeighbors(nodes, currentNode, target, closedSet, heuristic);
                if (nextNode == null) break;
                currentNode = nextNode;
            }
        
            return closedSet.Select(node => node.Position).ToList();
        }

        private Node ExploreNeighbors(Node[,] nodes, Node currentNode, Vector2Int target, List<Node> closedSet, ManagerUI.Heuristic heuristic)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        
            var neighbors = new List<Node>();
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = currentNode.Position + dir;
                if (IsValidNeighbor(nodes, neighborPos) && !closedSet.Contains(nodes[neighborPos.x, neighborPos.y]))
                {
                    Node neighborNode = nodes[neighborPos.x, neighborPos.y];
                    neighbors.Add(neighborNode);
                }
            }
            neighbors.Sort((a, b) => CalculateHCost(a.Position, target, heuristic).CompareTo(CalculateHCost(b.Position, target, heuristic)));
        
            return neighbors.Count > 0 ? neighbors[0] : null;
        }

        private bool IsValidNeighbor(Node[,] nodes, Vector2Int neighborPos)
        {
            var width = nodes.GetLength(0);
            var height = nodes.GetLength(1);
            return neighborPos.x >= 0 && neighborPos.x < width && neighborPos.y >= 0 && neighborPos.y < height && nodes[neighborPos.x, neighborPos.y].Traversable;
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
    }
}
