using System.Collections.Generic;
using UnityEngine;

namespace Pathfinders
{
    public interface IPathfinder
    {
        public List<Vector2Int> FindPath(Node[,] nodes, Vector2Int start, Vector2Int target, ManagerUI.Heuristic heuristic);
    }
}
