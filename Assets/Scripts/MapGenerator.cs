using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("General references")] 
    [SerializeField] private ManagerUI managerUI; // Ofc it's better to have some interface, and inject reference here, but for this demo straightforward ref is enough
    
    [Header("Tilemap references")]
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

    private void GenerateMap(int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), traversableTile);
            }
        }
    }
}
