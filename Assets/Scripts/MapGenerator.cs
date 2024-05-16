using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private Vector2Int startMapSize;
    
    [Header("General references")] 
    [SerializeField] private ManagerUI managerUI; // Ofc it's better to have some interface, and inject reference here, but for this demo straightforward ref is enough
    [SerializeField] private InputManager inputManager; // Same goes for here
    
    [Header("Grid references")] 
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile traversableTile;
    [SerializeField] private Tile obstacleTile;
    [SerializeField] private Tile wayTile;

    private (Vector3Int start, Vector3Int finish) _waypoints;
    private PathfinderStarA _pathfinder;
    private Node[,] _nodes;
    private bool _isPathGeneratingActive;

    private void OnEnable()
    {
        managerUI.OnMapGenerateClicked += GenerateMap;
        managerUI.OnGeneratePathClicked += GeneratePath;
        managerUI.OnStartPathGenerationClicked += StartPathGeneration;
        inputManager.OnGridClicked += ProcessGridClick;
    }

    private void OnDisable()
    {
        managerUI.OnMapGenerateClicked -= GenerateMap;
        managerUI.OnGeneratePathClicked -= GeneratePath;
        managerUI.OnStartPathGenerationClicked -= StartPathGeneration;
        inputManager.OnGridClicked -= ProcessGridClick;
    }

    private void Start()
    {
        GenerateMap(startMapSize.x, startMapSize.y);
        _pathfinder = new PathfinderStarA();
    }

    private void ProcessGridClick(Vector3 clickPos)
    {
        if (!_isPathGeneratingActive) return;

        var cellPosition = grid.WorldToCell(clickPos);

        if (_waypoints.start == Vector3Int.one)
        {
            _waypoints.start = cellPosition;
            tilemap.SetTile(_waypoints.start, wayTile);
            managerUI.ProgressPathfindingSequence(ManagerUI.PathfindingSequenceUI.ChooseFinishTile);
        }
        else if (_waypoints.finish == Vector3Int.one)
        {
            _waypoints.finish = cellPosition;
            tilemap.SetTile(_waypoints.finish, wayTile);
            managerUI.ProgressPathfindingSequence(ManagerUI.PathfindingSequenceUI.FindPath);
        }
    }

    private void GenerateMap(int width, int height)
    {
        ClearMap();
        
        _nodes = new Node[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), traversableTile);
                _nodes[i, j] = new Node(new Vector2Int(i, j), true);
            }
        }
        
        CenterCamera(width, height);
    }

    private void StartPathGeneration()
    {
        _isPathGeneratingActive = true;
    }

    private void GeneratePath()
    {
        var path = FindPath(
            new Vector2Int(_waypoints.start.x, _waypoints.start.y), 
            new Vector2Int(_waypoints.finish.x, _waypoints.finish.y));
        
        foreach (var node in path)
        {
            tilemap.SetTile(new Vector3Int(node.x, node.y, 0), wayTile);
        }
        
        _isPathGeneratingActive = false;
    }

    private void CenterCamera(int width, int height)
    {
        var cellGap = grid.cellGap;
        var centerCamX = (width + width * cellGap.x) / 2f;
        var centerCamY = (height + height * cellGap.y) / 2f;
        if (Camera.main != null) Camera.main.transform.position = new Vector3(centerCamX, 20, centerCamY);
    }
    
    private void ClearMap()
    {
        _waypoints = new ValueTuple<Vector3Int, Vector3Int>(Vector3Int.one, Vector3Int.one);
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            tilemap.SetTile(pos, null);
        }
    }

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        return _pathfinder.FindPath(_nodes, start, target);
    }
}
