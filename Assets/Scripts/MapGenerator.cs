using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private Vector2Int startMapSize;
    
    [Header("General references")] 
    [SerializeField] private ManagerUI managerUI; // Ofc it's better to have some interface, and inject reference here, but for this demo straightforward ref is enough

    [Header("Grid references")] 
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile traversableTile;
    [SerializeField] private Tile obstacleTile;

    private void OnEnable()
    {
        managerUI.OnMapGenerateClicked += GenerateMap;
    }

    private void OnDisable()
    {
        managerUI.OnMapGenerateClicked -= GenerateMap;
    }

    private void Start()
    {
        GenerateMap(startMapSize.x, startMapSize.y);
    }

    private void GenerateMap(int width, int height)
    {
        ClearMap();
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), traversableTile);
            }
        }
        
        CenterCamera(width, height);
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
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            tilemap.SetTile(pos, null);
        }
    }
}
